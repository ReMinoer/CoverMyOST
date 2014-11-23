using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CoverMyOST.GUI.Dialogs
{
    // TODO : fix album name edit
    internal class CoverSearchPresenter
    {
        private readonly CoverMyOSTClient _client;
        private readonly ICoverSearchView _view;

        private MusicFile _currentFile;
        private int _fileIndex;

        private int _lastSelection;

        private bool _playSong;
        private CoverSearchResult _searchResult;
        private SearchStatus _status;

        public CoverSearchPresenter(ICoverSearchView view, CoverMyOSTClient client)
        {
            _view = view;
            _client = client;
            _fileIndex = 0;

            _view.ListView.ItemSelectionChanged += ListViewOnItemSelectionChanged;

            _view.AlbumTextBox.KeyDown += AlbumTextBoxOnKeyDown;
            _view.AlbumTextBox.Leave += AlbumTextBoxOnLeave;
            _view.AlbumTextBox.GotFocus += AlbumTextBoxOnGotFocus;

            _view.PlayButton.Click += PlayButtonOnClick;
            _view.ApplyButton.Click += ApplyButtonOnClick;

            _view.BackgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _view.BackgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            _view.BackgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;

            _view.ClosingDialog += ViewOnClosingDialog;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            _status = SearchStatus.Init;

            _searchResult = new CoverSearchResult();
            _currentFile = _client.AllSelectedFiles.ElementAt(_fileIndex).Value;

            _view.ListView.Items.Clear();
            ListViewGroup defaultGroup = _view.ListView.Groups.Add("Default", "Default");
            _view.ListView.Groups.Add("Cached", "Cached");

            _view.ListView.Items.Add(new ListViewItem("*No cover*", defaultGroup));
            _view.ListView.Items.Add(new ListViewItem("*Actual cover*", defaultGroup));

            _view.ListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);

            _view.CoverNameLabel.Text = @"*Actual cover*";
            _view.ListView.Items[1].Selected = true;
            _lastSelection = 1;

            _view.CountLabel.Text = (_fileIndex + 1) + @"/" + _client.AllSelectedFiles.Count;
            _view.FileTextBox.Text = Path.GetFileName(_currentFile.Path);
            _view.AlbumTextBox.Text = _currentFile.Album;

            _view.CoverPreview.Image = _currentFile.Cover;

            _view.BackgroundWorker.RunWorkerAsync();

            Enabled(true);
            _view.ActiveControl = _view.AlbumTextBox;
            _view.AlbumTextBox.SelectionStart = 0;
            _view.AlbumTextBox.SelectionLength = _view.AlbumTextBox.TextLength;

            _status = SearchStatus.Search;

            if (_playSong)
                _client.PlayMusic(_currentFile.Path);
        }

        private void Enabled(bool state)
        {
            _view.ListView.Enabled = state;
            _view.AlbumTextBox.Enabled = state;
            _view.ApplyButton.Enabled = state;
            _view.PlayButton.Enabled = state;
        }

        private void PlayButtonOnClick(object sender, EventArgs eventArgs)
        {
            _playSong = !_playSong;

            if (_playSong)
            {
                _client.PlayMusic(_currentFile.Path);
                _view.PlayButton.Text = @"Stop";
            }
            else
            {
                _client.StopMusic();
                _view.PlayButton.Text = @"Play";
            }
        }

        private void ApplyButtonOnClick(object sender, EventArgs eventArgs)
        {
            switch (_status)
            {
                case SearchStatus.Search:
                    CancelSearch();
                    ApplyCover();
                    NextImage();
                    break;
                case SearchStatus.Wait:
                    ApplyCover();
                    if (NextImage())
                        InitializeDialog();
                    break;
            }
        }

        private void ListViewOnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            switch (e.ItemIndex)
            {
                case 0:
                    _view.CoverPreview.Image = null;
                    _view.CoverNameLabel.Text = @"*No cover*";
                    break;
                case 1:
                    _view.CoverPreview.Image = _currentFile.Cover;
                    _view.CoverNameLabel.Text = @"*Actual cover*";
                    break;
                default:
                    _view.CoverPreview.Image = _searchResult.ElementAt(e.ItemIndex - 2).Cover;
                    _view.CoverNameLabel.Text = _searchResult.ElementAt(e.ItemIndex - 2).Name;
                    break;
            }

            _lastSelection = e.ItemIndex;
        }

        private void AlbumTextBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Enter)
                if (_currentFile.Album != _view.AlbumTextBox.Text)
                {
                    _currentFile.Album = _view.AlbumTextBox.Text;

                    switch (_status)
                    {
                        case SearchStatus.Search:
                            CancelSearch();
                            break;
                        case SearchStatus.Wait:
                            InitializeDialog();
                            break;
                    }
                }
        }

        private void AlbumTextBoxOnLeave(object sender, EventArgs eventArgs)
        {
            _view.AlbumTextBox.Text = _currentFile.Album;
        }

        private void AlbumTextBoxOnGotFocus(object sender, EventArgs eventArgs)
        {
            _view.AlbumTextBox.SelectionStart = 0;
            _view.AlbumTextBox.SelectionLength = _view.AlbumTextBox.TextLength;
        }

        private void CancelSearch()
        {
            _view.BackgroundWorker.CancelAsync();

            Enabled(false);
            _status = SearchStatus.Cancel;

            _view.StatusLabel.Text = @"Waiting end of search...";
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
            if (_fileIndex >= _client.AllSelectedFiles.Count)
            {
                _view.CloseDialog();
                return false;
            }
            return true;
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker == null)
                return;

            int galleryCount = 0;
            foreach (ICoversGallery coversGallery in _client.Galleries)
            {
                if (!coversGallery.Enable)
                    continue;

                galleryCount++;
                if (coversGallery is OnlineGallery && (coversGallery as OnlineGallery).CacheEnable)
                    galleryCount++;
            }

            var progress = new SearchProgress
            {
                SearchResult = new CoverSearchResult(),
                GalleryName = _client.Galleries.ElementAt(0).Name,
                Cached = true
            };

            worker.ReportProgress(0, progress);

            int countProgress = 0;
            int i = 0;
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
            var searchProgress = ((SearchProgress)e.UserState);

            _view.SearchProgressBar.Value = e.ProgressPercentage;
            _view.StatusLabel.Text = string.Format("Search in {0}...", searchProgress.GalleryName);

            ListViewGroup group = null;
            if (!searchProgress.Cached)
                group = _view.ListView.Groups.Add(searchProgress.GalleryName, searchProgress.GalleryName);

            foreach (CoverEntry entry in searchProgress.SearchResult)
            {
                _searchResult.Add(entry);
                _view.ListView.Items.Add(searchProgress.Cached
                                             ? new ListViewItem(entry.GalleryName, _view.ListView.Groups["Cached"])
                                             : new ListViewItem(entry.Name, group));
            }

            _view.ListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                InitializeDialog();
                return;
            }

            if (e.Error != null)
            {
                _view.SearchProgressBar.Value = 0;
                _view.StatusLabel.Text = (@"Error: " + e.Error.Message);
            }
            else
            {
                _view.SearchProgressBar.Value = 100;
                _view.StatusLabel.Text = @"Search completed.";
            }

            if (_status == SearchStatus.Cancel)
            {
                Enabled(true);
                InitializeDialog();
            }
            _status = SearchStatus.Wait;
        }

        private void ViewOnClosingDialog(object sender, CancelEventArgs cancelEventArgs)
        {
            CancelSearch();
            _client.StopMusic();
        }

        private struct SearchProgress
        {
            public CoverSearchResult SearchResult { get; set; }
            public string GalleryName { get; set; }
            public bool Cached { get; set; }
        }

        private enum SearchStatus
        {
            Init,
            Search,
            Wait,
            Cancel
        }
    }
}