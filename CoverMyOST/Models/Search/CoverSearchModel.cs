using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CoverMyOST.Galleries.Base;
using CoverMyOST.Models.Galleries;

namespace CoverMyOST.Models.Search
{
    public class CoverSearchModel
    {
        private readonly GalleryManager _galleryManager;
        private CancellationTokenSource _cancellationTokenSource;
        private CoverSearchResult _searchResult;
        public CoverSearchState State { get; private set; }

        public IReadOnlyList<CoverEntry> SearchResults
        {
            get { return _searchResult.AsReadOnly(); }
        }

        public event EventHandler SearchLaunch;
        public event EventHandler<SearchProgressEventArgs> SearchProgress;
        public event EventHandler SearchCancel;
        public event EventHandler<SearchErrorEventArgs> SearchError;
        public event EventHandler SearchSuccess;
        public event EventHandler SearchEnd;

        public CoverSearchModel(GalleryManager galleryManager)
        {
            _galleryManager = galleryManager;
        }

        public async void LaunchSearch(string albumName)
        {
            if (State != CoverSearchState.Wait)
                throw new InvalidOperationException("New search can't be launch while another is processing or canceled.");

            _searchResult = new CoverSearchResult();

            if (albumName == "")
                return;

            var progressHandler = new Progress<CoverSearchStatus>();
            progressHandler.ProgressChanged += ProgressHandlerOnProgressChanged;

            IProgress<CoverSearchStatus> progress = progressHandler;

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = _cancellationTokenSource.Token;

            IEnumerable<CoversGallery> galleries = _galleryManager.GetAllComponentsInChildren<CoversGallery>().ToList();

            var status = new CoverSearchStatus
            {
                Progress = 0,
                SearchResult = new CoverSearchResult(),
                GalleryName = galleries.ElementAt(0).Name,
                Cached = true
            };

            State = CoverSearchState.Search;

            if (SearchLaunch != null)
                SearchLaunch.Invoke(this, EventArgs.Empty);

            int galleryCount = 0;
            foreach (ICoversGallery coversGallery in galleries)
            {
                if (!coversGallery.Enable)
                    continue;

                galleryCount++;
                if (coversGallery is OnlineGallery && (coversGallery as OnlineGallery).UseCache)
                    galleryCount++;
            }

            try
            {
                ct.ThrowIfCancellationRequested();

                progress.Report(status);

                double countProgress = 0;
                int i = 0;
                foreach (ICoversGallery gallery in galleries)
                {
                    if (gallery.Enable && gallery is OnlineGallery && (gallery as OnlineGallery).UseCache)
                    {
                        status.Progress = countProgress / galleryCount;
                        status.GalleryName = galleries.ElementAt(i).Name + " (cache)";
                        status.SearchResult = new CoverSearchResult();
                        progress.Report(status);

                        CoverEntry entry = (gallery as OnlineGallery).SearchCached(albumName);

                        status.SearchResult = new CoverSearchResult();
                        if (entry != null)
                        {
                            status.SearchResult.Add(entry);
                            _searchResult.Add(entry);
                        }

                        i++;
                        countProgress++;

                        status.Progress = countProgress / galleryCount;
                        progress.Report(status);
                    }

                    ct.ThrowIfCancellationRequested();
                }

                i = 0;
                status.Cached = false;

                foreach (ICoversGallery gallery in galleries)
                {
                    if (gallery.Enable)
                    {
                        status.Progress = countProgress / galleryCount;
                        status.GalleryName = galleries.ElementAt(i).Name;
                        status.SearchResult = new CoverSearchResult();
                        progress.Report(status);

                        status.SearchResult = gallery is OnlineGallery
                            ? await (gallery as OnlineGallery).SearchOnlineAsync(albumName, _cancellationTokenSource.Token)
                            : await gallery.SearchAsync(albumName, _cancellationTokenSource.Token);

                        foreach (CoverEntry entry in status.SearchResult)
                            _searchResult.Add(entry);

                        i++;
                        countProgress++;

                        status.Progress = countProgress / galleryCount;
                        progress.Report(status);
                    }

                    ct.ThrowIfCancellationRequested();
                }

                State = CoverSearchState.Wait;

                if (SearchSuccess != null)
                    SearchSuccess.Invoke(this, EventArgs.Empty);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                var args = new SearchErrorEventArgs
                {
                    ErrorMessage = e.Message
                };

                if (SearchError != null)
                    SearchError.Invoke(this, args);
            }
            finally
            {
                State = CoverSearchState.Wait;

                if (SearchEnd != null)
                    SearchEnd.Invoke(this, EventArgs.Empty);
            }
        }

        public void CancelSearch()
        {
            if (State != CoverSearchState.Search)
                throw new InvalidOperationException("There is no search currently running.");

            State = CoverSearchState.Cancel;

            if (SearchCancel != null)
                SearchCancel.Invoke(this, EventArgs.Empty);

            _cancellationTokenSource.Cancel();
        }

        private void ProgressHandlerOnProgressChanged(object sender, CoverSearchStatus coverSearchStatus)
        {
            var args = new SearchProgressEventArgs
            {
                Status = coverSearchStatus
            };

            if (SearchProgress != null)
                SearchProgress.Invoke(sender, args);
        }
    }

    public enum CoverSearchState
    {
        Wait,
        Search,
        Cancel
    }

    public struct CoverSearchStatus
    {
        public double Progress { get; set; }
        public CoverSearchResult SearchResult { get; set; }
        public string GalleryName { get; set; }
        public bool Cached { get; set; }
    }

    public struct SearchProgressEventArgs
    {
        public CoverSearchStatus Status { get; set; }
    }

    public class SearchErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }
}