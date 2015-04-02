using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace CoverMyOST.GUI.Dialogs
{
    public class CoverSearchModel
    {
        private readonly BackgroundWorker _backgroundWorker;
        private readonly CoverMyOSTClient _client;
        private int _indexSelected;
        private CoverSearchResult _searchResult;
        public CoverSearchStep Step { get; private set; }
        public MusicFile CurrentFile { get; private set; }
        public int FileIndex { get; private set; }
        public Bitmap CoverSelected { get; private set; }
        public bool IsPlayingSong { get; private set; }

        public int FilesCount
        {
            get { return _client.AllSelectedFiles.Count; }
        }

        public event EventHandler Initialize;
        public event EventHandler<ProgressChangedEventArgs> SearchProgress;
        public event EventHandler SearchCancel;
        public event EventHandler<SearchErrorEventArgs> SearchError;
        public event EventHandler SearchComplete;
        public event EventHandler SearchEnd;
        public event EventHandler ProcessEnd;

        public CoverSearchModel(CoverMyOSTClient client)
        {
            _client = client;

            _backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};

            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
        }

        public void Reset()
        {
            Step = CoverSearchStep.Init;

            _searchResult = new CoverSearchResult();
            CurrentFile = _client.AllSelectedFiles.ElementAt(FileIndex).Value;

            _backgroundWorker.RunWorkerAsync();

            Step = CoverSearchStep.Search;
            CoverSelected = CurrentFile.Cover;

            if (IsPlayingSong)
                _client.PlayMusic(CurrentFile.Path);

            if (Initialize != null)
                Initialize.Invoke(this, EventArgs.Empty);
        }

        public void ToggleSong()
        {
            IsPlayingSong = !IsPlayingSong;

            if (IsPlayingSong)
                _client.PlayMusic(CurrentFile.Path);
            else
                _client.StopMusic();
        }

        public void Apply()
        {
            switch (Step)
            {
                case CoverSearchStep.Search:
                    CancelSearch();
                    ApplyCover();
                    NextImage();
                    break;
                case CoverSearchStep.Wait:
                    ApplyCover();
                    if (NextImage())
                        Reset();
                    break;
            }
        }

        public void EditAlbum(string albumName)
        {
            if (CurrentFile.Album != albumName)
            {
                CurrentFile.Album = albumName;

                switch (Step)
                {
                    case CoverSearchStep.Search:
                        CancelSearch();
                        break;
                    case CoverSearchStep.Wait:
                        Reset();
                        break;
                }
            }
        }

        public void Close()
        {
            CancelSearch();
            _client.StopMusic();

            // BUG : Client continue to search if dialog close.
        }

        public void ChangeCoverSelected(int index)
        {
            if (Step == CoverSearchStep.Init)
                return;

            CoverSelected = _searchResult.ElementAt(index).Cover;
            _indexSelected = index;
        }

        public void ResetCoverSelected()
        {
            if (Step == CoverSearchStep.Init)
                return;

            CoverSelected = CurrentFile.Cover;
            _indexSelected = -1;
        }

        public void RemoveCoverSelected()
        {
            if (Step == CoverSearchStep.Init)
                return;

            CoverSelected = null;
            _indexSelected = -1;
        }

        private void CancelSearch()
        {
            _backgroundWorker.CancelAsync();
            Step = CoverSearchStep.Cancel;

            if (SearchCancel != null)
                SearchCancel.Invoke(this, EventArgs.Empty);
        }

        private void ApplyCover()
        {
            CurrentFile.Cover = CoverSelected;
            if (_indexSelected != -1)
            {
                CoverEntry entry = _searchResult.ElementAt(_indexSelected);
                entry.AddToGalleryCache(CurrentFile.Album);
            }
        }

        private bool NextImage()
        {
            FileIndex++;
            if (FileIndex < _client.AllSelectedFiles.Count)
                return true;

            if (ProcessEnd != null)
                ProcessEnd.Invoke(this, EventArgs.Empty);

            return false;
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

                    CoverEntry entry = (gallery as OnlineGallery).SearchCached(CurrentFile.Album);
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
                        ? (gallery as OnlineGallery).SearchOnline(CurrentFile.Album)
                        : gallery.Search(CurrentFile.Album);
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

            // WATCH : Is selection updated ?
            //if (firstCached)
            //    _data.LastSelection = 2;

            if (SearchProgress != null)
                SearchProgress.Invoke(this, e);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Reset();
                return;
            }

            if (e.Error != null)
            {
                var aggregateException = e.Error as AggregateException;
                string errorMessage = aggregateException != null
                    ? aggregateException.InnerExceptions[0].Message
                    : e.Error.Message;

                if (SearchError != null)
                    SearchError.Invoke(this, new SearchErrorEventArgs {ErrorMessage = errorMessage});
            }
            else if (SearchComplete != null)
                SearchComplete.Invoke(this, EventArgs.Empty);

            if (SearchEnd != null)
                SearchEnd.Invoke(this, EventArgs.Empty);

            if (Step == CoverSearchStep.Cancel && FileIndex < _client.AllSelectedFiles.Count)
                Reset();

            Step = CoverSearchStep.Wait;
        }
    }

    public enum CoverSearchStep
    {
        Init,
        Search,
        Wait,
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