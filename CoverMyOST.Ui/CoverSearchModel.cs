using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CoverMyOST.Ui
{
    public class CoverSearchModel
    {
        private readonly BackgroundWorker _backgroundWorker;
        private readonly CoverMyOSTClient _client;
        private CoverSearchResult _searchResult;
        private string _albumName;

        public CoverSearchState State { get; private set; }

        public IReadOnlyList<CoverEntry> SearchResults
        {
            get { return new ReadOnlyCollection<CoverEntry>(_searchResult.ToList());}
        }

        public event EventHandler SearchLaunch;
        public event EventHandler<ProgressChangedEventArgs> SearchProgress;
        public event EventHandler SearchCancel;
        public event EventHandler<SearchErrorEventArgs> SearchError;
        public event EventHandler SearchSuccess;
        public event EventHandler SearchEnd;

        public CoverSearchModel(CoverMyOSTClient client)
        {
            _client = client;

            _backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};

            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
        }

        public void LaunchSearch(string albumName)
        {
            if (State != CoverSearchState.Wait)
                throw new InvalidOperationException("New search can't be launch while another is processing or canceled.");

            _albumName = albumName;
            _searchResult = new CoverSearchResult();

            if (albumName == "")
                return;

            _backgroundWorker.RunWorkerAsync();

            State = CoverSearchState.Search;

            if (SearchLaunch != null)
                SearchLaunch.Invoke(this, EventArgs.Empty);
        }

        public void CancelSearch()
        {
            if (State != CoverSearchState.Search)
                throw new InvalidOperationException("There is no search currently running.");

            _backgroundWorker.CancelAsync();
            State = CoverSearchState.Cancel;

            if (SearchCancel != null)
                SearchCancel.Invoke(this, EventArgs.Empty);
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker == null)
                return;

            var galleryCount = 0;
            foreach (ICoversGallery coversGallery in _client.Galleries)
            {
                if (!coversGallery.Enable)
                    continue;

                galleryCount++;
                if (coversGallery is OnlineGallery && (coversGallery as OnlineGallery).CacheEnable)
                    galleryCount++;
            }

            var progress = new CoverSearchProgress
            {
                SearchResult = new CoverSearchResult(),
                GalleryName = _client.Galleries.ElementAt(0).Name,
                Cached = true
            };

            worker.ReportProgress(0, progress);

            var countProgress = 0;
            var i = 0;
            foreach (ICoversGallery gallery in _client.Galleries)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if (gallery.Enable && gallery is OnlineGallery && (gallery as OnlineGallery).CacheEnable)
                {
                    progress.GalleryName = _client.Galleries.ElementAt(i).Name + " (cache)";
                    progress.SearchResult = new CoverSearchResult();
                    worker.ReportProgress((int)(countProgress * (100.0 / galleryCount)), progress);

                    CoverEntry entry = (gallery as OnlineGallery).SearchCached(_albumName);
                    progress.SearchResult = entry != null ? new CoverSearchResult(entry) : new CoverSearchResult();

                    i++;
                    countProgress++;
                    worker.ReportProgress((int)(countProgress * (100.0 / galleryCount)), progress);
                }
            }

            i = 0;
            progress.Cached = false;

            foreach (ICoversGallery gallery in _client.Galleries)
            {
                if (worker.CancellationPending)
                {
                    if (gallery is OnlineGallery)
                        (gallery as OnlineGallery).CancelSearch();
                    e.Cancel = true;
                    break;
                }

                if (gallery.Enable)
                {
                    progress.GalleryName = _client.Galleries.ElementAt(i).Name;
                    progress.SearchResult = new CoverSearchResult();
                    worker.ReportProgress((int)(countProgress * (100.0 / galleryCount)), progress);

                    progress.SearchResult = gallery is OnlineGallery
                        ? (gallery as OnlineGallery).SearchOnline(_albumName)
                        : gallery.Search(_albumName);
                    worker.ReportProgress((int)(countProgress * (100.0 / galleryCount)), progress);

                    i++;
                    countProgress++;
                }
            }

            if (worker.CancellationPending)
                e.Cancel = true;
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var searchProgress = ((CoverSearchProgress)e.UserState);

            foreach (CoverEntry entry in searchProgress.SearchResult)
                _searchResult.Add(entry);

            if (SearchProgress != null)
                SearchProgress.Invoke(this, e);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error != null)
                {
                    var aggregateException = e.Error as AggregateException;
                    string errorMessage = aggregateException != null
                        ? aggregateException.InnerExceptions[0].Message
                        : e.Error.Message;

                    if (SearchError != null)
                        SearchError.Invoke(this, new SearchErrorEventArgs {ErrorMessage = errorMessage});
                }
                else if (SearchSuccess != null)
                    SearchSuccess.Invoke(this, EventArgs.Empty);
            }

            State = CoverSearchState.Wait;

            if (SearchEnd != null)
                SearchEnd.Invoke(this, EventArgs.Empty);
        }
    }

    public enum CoverSearchState
    {
        Wait,
        Search,
        Cancel
    }

    public struct CoverSearchProgress
    {
        public CoverSearchResult SearchResult { get; set; }
        public string GalleryName { get; set; }
        public bool Cached { get; set; }
    }

    public class SearchErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }
}