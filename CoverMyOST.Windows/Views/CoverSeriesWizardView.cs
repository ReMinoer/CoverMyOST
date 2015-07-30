using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CoverMyOST.Ui;

namespace CoverMyOST.Windows.Views
{
    public partial class CoverSeriesWizardView : Form
    {
        public event EventHandler<EditAlbumRequestEventArgs> EditAlbumRequest;
        public event EventHandler ResetAlbumRequest;
        public event EventHandler<CoverSelectionEventArgs> CoverSelectionRequest;
        public event EventHandler ApplyRequest;
        public event EventHandler ToggleSongRequest;

        public CoverSeriesWizardView()
        {
            InitializeComponent();

            _playButton.Click += PlayButtonOnClick;
            _applyButton.Click += ApplyButtonOnClick;

            _albumTextBox.KeyDown += AlbumTextBoxOnKeyDown;
            _albumTextBox.Leave += AlbumTextBoxOnLeave;
            _albumTextBox.GotFocus += AlbumTextBoxOnGotFocus;

            _listView.ItemSelectionChanged += ListViewOnItemSelectionChanged;
        }

        public void Initialize(string filename, Bitmap cover, string album, int fileIndex, int filesCount)
        {
            _listView.Items.Clear();
            _listView.Groups.Clear();

            ListViewGroup defaultGroup = _listView.Groups.Add("Default", "Default");
            _listView.Groups.Add("Cached", "Cached");

            _listView.Items.Add(new ListViewItem("*No cover*", defaultGroup));
            _listView.Items.Add(new ListViewItem("*Actual cover*", defaultGroup));

            _listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);

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

        public void SearchProgress(CoverSearchProgress searchProgress, int progressPercentage)
        {
            _searchProgressBar.Value = progressPercentage;
            _statusLabel.Text = string.Format("Search in {0}...", searchProgress.GalleryName);

            bool firstCached = false;
            ListViewGroup group = null;

            if (!searchProgress.Cached)
                group = _listView.Groups.Add(searchProgress.GalleryName, searchProgress.GalleryName);

            foreach (CoverEntry entry in searchProgress.SearchResult)
            {
                if (searchProgress.Cached && _listView.Groups["Cached"].Items.Count == 0)
                    firstCached = true;

                _listView.Items.Add(searchProgress.Cached
                    ? new ListViewItem(entry.GalleryName, _listView.Groups["Cached"])
                    : new ListViewItem(entry.Name, group));
            }

            if (firstCached)
                _listView.Items[2].Selected = true;

            _listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public void SearchCancel()
        {
            EnabledForm(false);
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

        public void SearchEnd(bool enableForm)
        {
            EnabledForm(enableForm);
        }

        private void ApplyButtonOnClick(object sender, EventArgs e)
        {
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
                    _coverNameLabel.Text = string.Format("[{0}] {1}", item.Group.Name, item.Name);
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