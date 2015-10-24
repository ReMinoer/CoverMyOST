using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CoverMyOST.Searchers;

namespace CoverMyOST.Windows.Dialogs.Views
{
    public partial class CoverSeriesWizardView : Form
    {
        private readonly ImageList _imageList;
        public event EventHandler<EditAlbumRequestEventArgs> EditAlbumRequest;
        public event EventHandler ResetAlbumRequest;
        public event EventHandler<CoverSelectionEventArgs> CoverSelectionRequest;
        public event EventHandler ApplyRequest;
        public event EventHandler ToggleSongRequest;

        public CoverSeriesWizardView()
        {
            InitializeComponent();

            _imageList = new ImageList
            {
                ImageSize = new Size(50, 70),
                ColorDepth = ColorDepth.Depth24Bit
            };
            _listView.LargeImageList = _imageList;

            _playButton.Click += PlayButtonOnClick;
            _applyButton.Click += ApplyButtonOnClick;

            _albumTextBox.KeyDown += AlbumTextBoxOnKeyDown;
            _albumTextBox.Leave += AlbumTextBoxOnLeave;
            _albumTextBox.GotFocus += AlbumTextBoxOnGotFocus;

            _listView.ItemSelectionChanged += ListViewOnItemSelectionChanged;
            
            Closing += OnClosing;
        }

        public void Initialize(string filename, Bitmap cover, string album, int fileIndex, int filesCount)
        {
            _listView.ItemSelectionChanged -= ListViewOnItemSelectionChanged;

            _listView.Items.Clear();
            _listView.Groups.Clear();

            ListViewGroup defaultGroup = _listView.Groups.Add("Default", "Default");
            _listView.Groups.Add("Cached", "Cached");

            _listView.Items.Add(new ListViewItem("*No cover*", defaultGroup));
            _listView.Items.Add(new ListViewItem("*Actual cover*", defaultGroup));

            _listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            _imageList.Images.Clear();

            _coverNameLabel.Text = @"*Actual cover*";

            _countLabel.Text = (fileIndex + 1) + @"/" + filesCount;
            _fileTextBox.Text = Path.GetFileName(filename);
            _albumTextBox.Text = album;

            _coverPreview.Image = cover;

            EnabledForm(true);
            ActiveControl = _albumTextBox;
            _albumTextBox.SelectionStart = 0;
            _albumTextBox.SelectionLength = _albumTextBox.TextLength;

            _listView.Items[1].Selected = true;
            _listView.ItemSelectionChanged += ListViewOnItemSelectionChanged;
        }

        public void UpdatePlayMusicButton(bool isPlaying)
        {
            _playButton.Text = isPlaying ? @"Stop" : @"Play";
        }

        public void ChangeAlbumName(string albumName)
        {
            _albumTextBox.Text = albumName;
        }

        public void ChangeCover(Bitmap cover)
        {
            _coverPreview.Image = cover;
        }

        public void SearchProgress(CoverSearchStatus searchStatus)
        {
            _searchProgressBar.Value = (int)(searchStatus.Progress * 100);
            _statusLabel.Text = string.Format("Search in {0}...", searchStatus.GalleryName);

            bool firstCached = false;
            ListViewGroup group = null;

            if (!searchStatus.Cached)
                group = _listView.Groups.Add(searchStatus.GalleryName, searchStatus.GalleryName);

            foreach (CoverEntry entry in searchStatus.SearchResult)
            {
                if (searchStatus.Cached && _listView.Groups["Cached"].Items.Count == 0)
                    firstCached = true;

                ListViewItem item = searchStatus.Cached
                    ? new ListViewItem(entry.GalleryName, _listView.Groups["Cached"])
                    : new ListViewItem(entry.Name, group);

                _imageList.Images.Add(entry.Cover);
                item.ImageIndex = _imageList.Images.Count - 1;

                _listView.Items.Add(item);
            }

            if (firstCached)
                _listView.Items[2].Selected = true;

            _listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public void SearchCancel()
        {
            _statusLabel.Text = @"Waiting end of search...";
        }

        public void SearchError(string errorMessage)
        {
            _searchProgressBar.Value = 0;
            _statusLabel.Text = errorMessage;
        }

        public void SearchSuccess()
        {
            _searchProgressBar.Value = 100;
            _statusLabel.Text = @"Search completed.";
        }

        public void SearchEnd(CoverSearchState state)
        {
            EnabledForm(state != CoverSearchState.Cancel);
        }

        private void ApplyButtonOnClick(object sender, EventArgs e)
        {
            EnabledForm(false);

            if (ApplyRequest != null)
                ApplyRequest(sender, e);
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            if (ToggleSongRequest != null)
                ToggleSongRequest(sender, e);
        }

        private void AlbumTextBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Enter && EditAlbumRequest != null)
                EditAlbumRequest.Invoke(this, new EditAlbumRequestEventArgs
                {
                    AlbumName = _albumTextBox.Text
                });
        }

        private void AlbumTextBoxOnLeave(object sender, EventArgs e)
        {
            if (ResetAlbumRequest != null)
                ResetAlbumRequest(sender, e);
        }

        private void AlbumTextBoxOnGotFocus(object sender, EventArgs eventArgs)
        {
            _albumTextBox.SelectionStart = 0;
            _albumTextBox.SelectionLength = _albumTextBox.TextLength;
        }

        private void ListViewOnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var args = new CoverSelectionEventArgs();

            switch (e.ItemIndex)
            {
                case 0:
                    args.Action = CoverSelectionAction.Remove;
                    args.SelectionIndex = -1;
                    _coverNameLabel.Text = @"*No cover*";
                    break;
                case 1:
                    args.Action = CoverSelectionAction.Reset;
                    args.SelectionIndex = -1;
                    _coverNameLabel.Text = @"*Actual cover*";
                    break;
                default:
                    args.Action = CoverSelectionAction.Change;
                    args.SelectionIndex = e.ItemIndex - 2;
                    ListViewItem item = _listView.Items[e.ItemIndex];
                    _coverNameLabel.Text = string.Format("[{0}] {1}", item.Group.Name, item.Text);
                    break;
            }

            if (CoverSelectionRequest != null)
                CoverSelectionRequest.Invoke(this, args);
        }

        private void EnabledForm(bool state)
        {
            _listView.Enabled = state;
            _albumTextBox.Enabled = state;
            _applyButton.Enabled = state;
            _playButton.Enabled = state;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _listView.ItemSelectionChanged -= ListViewOnItemSelectionChanged;
        }

        public enum CoverSelectionAction
        {
            Remove,
            Reset,
            Change
        }

        public class EditAlbumRequestEventArgs : EventArgs
        {
            public string AlbumName { get; set; }
        }

        public class CoverSelectionEventArgs : EventArgs
        {
            public CoverSelectionAction Action { get; set; }
            public int SelectionIndex { get; set; }
        }
    }
}