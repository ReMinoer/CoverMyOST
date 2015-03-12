using System;
using System.ComponentModel;
using System.Linq;

namespace CoverMyOST.GUI.Dialogs
{
    public class CoverSearchModel : CoverSearchUi.IModel
    {
        private readonly BackgroundWorker _backgroundWorker;
        private readonly CoverMyOSTClient _client;
        private MusicFile _currentFile;
        private int _fileIndex;
        private bool _isPlayingSong;
        private int _lastSelection;
        private CoverSearchResult _searchResult;
        private CoverSearchStep _step;
        public event EventHandler<CoverSearchUi.InitializeEventArgs> Initialize;
        public event EventHandler<string> ResetAlbum;
        public event EventHandler<CoverSearchUi.CoverChangeEventArgs> CoverChange;
        public event EventHandler<ProgressChangedEventArgs> SearchProgress;
        public event EventHandler SearchCancel;
        public event EventHandler<string> SearchError;
        public event EventHandler SearchComplete;
        public event EventHandler<bool> SearchEnd;
        public event EventHandler<bool> PlaySong;
        public event EventHandler Close;

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
            _step = CoverSearchStep.Init;

            _searchResult = new CoverSearchResult();
            _currentFile = _client.AllSelectedFiles.ElementAt(_fileIndex).Value;

            _backgroundWorker.RunWorkerAsync();

            _step = CoverSearchStep.Search;
            _lastSelection = 1;

            if (_isPlayingSong)
                _client.PlayMusic(_currentFile.Path);

            var args = new CoverSearchUi.InitializeEventArgs
            {
                FileIndex = _fileIndex,
                FilesCount = _client.AllSelectedFiles.Count,
                CurrentFile = _currentFile
            };

            if (Initialize != null)
                Initialize.Invoke(this, args);
        }

        public void OnPlaySongRequest(object sender, EventArgs e)
        {
            _isPlayingSong = !_isPlayingSong;

            if (_isPlayingSong)
                _client.PlayMusic(_currentFile.Path);
            else
                _client.StopMusic();

            if (PlaySong != null)
                PlaySong.Invoke(this, _isPlayingSong);
        }

        public void OnApplyRequest(object sender, EventArgs e)
        {
            switch (_step)
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

        public void OnEditAlbumRequest(object sender, string albumName)
        {
            if (_currentFile.Album != albumName)
            {
                _currentFile.Album = albumName;

                switch (_step)
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

        public void OnResetAlbumRequest(object sender, EventArgs e)
        {
            if (ResetAlbum != null)
                ResetAlbum.Invoke(this, _currentFile.Album);
        }

        public void OnCloseRequest(object sender, EventArgs e)
        {
            CancelSearch();
            _client.StopMusic();

            // BUG : Client continue to search if dialog close.
        }

        public void OnCoverSelectionRequest(object sender, CoverSearchUi.CoverSelectionEventArgs e)
        {
            if (_step == CoverSearchStep.Init)
                return;

            var args = new CoverSearchUi.CoverChangeEventArgs();

            switch (e.Index)
            {
                case 0:
                    args.Cover = null;
                    args.Name = @"*No cover*";
                    break;
                case 1:
                    args.Cover = _currentFile.Cover;
                    args.Name = @"*Actual cover*";
                    break;
                default:
                    args.Cover = _searchResult.ElementAt(e.Index - 2).Cover;
                    args.Name = string.Format("[{0}] {1}", e.Group, _searchResult.ElementAt(e.Index - 2).Name);
                    break;
            }

            _lastSelection = e.Index;

            if (CoverChange != null)
                CoverChange.Invoke(this, args);
        }

        private void CancelSearch()
        {
            _backgroundWorker.CancelAsync();
            _step = CoverSearchStep.Cancel;

            if (SearchCancel != null)
                SearchCancel.Invoke(this, EventArgs.Empty);
        }

        private void ApplyCover()
        {
            if (_lastSelection == 0)
                _currentFile.Cover = null;
            else if (_lastSelection != 1)
            {
                CoverEntry entry = _searchResult.ElementAt(_lastSelection - 2);
                _currentFile.Cover = entry.Cover;
                entry.AddToGalleryCache(_currentFile.Album);
            }
        }

        private bool NextImage()
        {
            _fileIndex++;
            if (_fileIndex < _client.AllSelectedFiles.Count)
                return true;

            if (Close != null)
                Close.Invoke(this, EventArgs.Empty);

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

                    CoverEntry entry = (gallery as OnlineGallery).SearchCached(_currentFile.Album);
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
                        ? (gallery as OnlineGallery).SearchOnline(_currentFile.Album)
                        : gallery.Search(_currentFile.Album);
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
                    SearchError.Invoke(this, errorMessage);
            }
            else if (SearchComplete != null)
                SearchComplete.Invoke(this, EventArgs.Empty);

            if (SearchEnd != null)
                SearchEnd.Invoke(this, _step != CoverSearchStep.Cancel);

            if (_step == CoverSearchStep.Cancel && _fileIndex < _client.AllSelectedFiles.Count)
                Reset();

            _step = CoverSearchStep.Wait;
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
}