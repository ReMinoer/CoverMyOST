﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace CoverMyOST.GUI.Dialogs
{
    public partial class CoverSearchView : Form, CoverSearchUi.IView
    {
        public event EventHandler<string> EditAlbumRequest;
        public event EventHandler ResetAlbumRequest;
        public event EventHandler<CoverSearchUi.CoverSelectionEventArgs> CoverSelectionRequest;
        public event EventHandler ApplyRequest;
        public event EventHandler PlaySongRequest;
        public event EventHandler CloseRequest;

        public CoverSearchView()
        {
            InitializeComponent();

            _playButton.Click += PlayButtonOnClick;
            _applyButton.Click += ApplyButtonOnClick;

            _albumTextBox.KeyDown += AlbumTextBoxOnKeyDown;
            _albumTextBox.Leave += AlbumTextBoxOnLeave;
            _albumTextBox.GotFocus += AlbumTextBoxOnGotFocus;

            _listView.ItemSelectionChanged += ListViewOnItemSelectionChanged;

            Closing += OnClosing;
        }

        public void Show(bool stayFocus)
        {
            if (stayFocus)
                ShowDialog();
            else
                Show();
        }

        public void OnInitialize(object sender, CoverSearchUi.InitializeEventArgs e)
        {
            _listView.Items.Clear();
            _listView.Groups.Clear();

            ListViewGroup defaultGroup = _listView.Groups.Add("Default", "Default");
            _listView.Groups.Add("Cached", "Cached");

            _listView.Items.Add(new ListViewItem("*No cover*", defaultGroup));
            _listView.Items.Add(new ListViewItem("*Actual cover*", defaultGroup));

            _listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);

            _coverNameLabel.Text = @"*Actual cover*";

            _countLabel.Text = (e.FileIndex + 1) + @"/" + e.FilesCount;
            _fileTextBox.Text = Path.GetFileName(e.CurrentFile.Path);
            _albumTextBox.Text = e.CurrentFile.Album;

            _coverPreview.Image = e.CurrentFile.Cover;

            Enabled(true);
            ActiveControl = _albumTextBox;
            _albumTextBox.SelectionStart = 0;
            _albumTextBox.SelectionLength = _albumTextBox.TextLength;

            _listView.Items[1].Selected = true;
        }

        public void OnPlaySong(object sender, bool isPlaying)
        {
            _playButton.Text = isPlaying ? @"Stop" : @"Play";
        }

        public void OnSearchCancel(object sender, EventArgs e)
        {
            Enabled(false);
            _statusLabel.Text = @"Waiting end of search...";
        }

        public void OnResetAlbum(object sender, string albumName)
        {
            _albumTextBox.Text = albumName;
        }

        public void OnCoverChange(object sender, CoverSearchUi.CoverChangeEventArgs e)
        {
            _coverPreview.Image = e.Cover;
            _coverNameLabel.Text = e.Name;
        }

        public void OnSearchProgress(object sender, ProgressChangedEventArgs e)
        {
            var searchProgress = ((CoverSearchProgress)e.UserState);

            _searchProgressBar.Value = e.ProgressPercentage;
            _statusLabel.Text = string.Format("Search in {0}...", searchProgress.GalleryName);

            var firstCached = false;
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

        public void OnSearchError(object sender, string errorMessage)
        {
            _searchProgressBar.Value = 0;
            _statusLabel.Text = errorMessage;
        }

        public void OnSearchComplete(object sender, EventArgs e)
        {
            _searchProgressBar.Value = 100;
            _statusLabel.Text = @"Search completed.";
        }

        public void OnSearchEnd(object sender, bool enableForm)
        {
            Enabled(enableForm);
        }

        public void OnClose(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyButtonOnClick(object sender, EventArgs e)
        {
            if (ApplyRequest != null)
                ApplyRequest(sender, e);
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            if (PlaySongRequest != null)
                PlaySongRequest(sender, e);
        }

        private void AlbumTextBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Enter && EditAlbumRequest != null)
                EditAlbumRequest.Invoke(this, _albumTextBox.Text);
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
            if (CoverSelectionRequest == null)
                return;

            var args = new CoverSearchUi.CoverSelectionEventArgs
            {
                Index = e.ItemIndex,
                Group = _listView.Items[e.ItemIndex].Group.Name
            };

            CoverSelectionRequest.Invoke(this, args);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (CloseRequest != null)
                CloseRequest(sender, e);
        }

        private void Enabled(bool state)
        {
            _listView.Enabled = state;
            _albumTextBox.Enabled = state;
            _applyButton.Enabled = state;
            _playButton.Enabled = state;
        }
    }
}