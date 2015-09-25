using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CoverMyOST.FileManagers;

namespace CoverMyOST.Windows
{
    public partial class MainView : Form
    {
        private bool _isSaved = true;
        public event EventHandler<ChangeFolderRequestArgs> ChangeFolderRequest;
        public event EventHandler SaveAllRequest;
        public event EventHandler<ChangeFilterRequestArgs> ChangeFilterRequest;
        public event EventHandler ShowGalleryManagerRequest;
        public event EventHandler ShowCoverSeriesWizardRequest;
        public event EventHandler<SelectedFileChangedArgs> SelectedFilesChanged;
        public event EventHandler<AlbumNameChangedArgs> AlbumNameChanged;
        public event EventHandler<PlayMusicArgs> PlayMusicRequest;
        public event EventHandler StopMusicRequest;
        public event EventHandler<CloseRequestArgs> CloseRequest;

        public MainView()
        {
            InitializeComponent();

            stopButton.Enabled = false;

            foreach (string name in Enum.GetNames(typeof(MusicFileFilter)))
                filterComboBox.Items.Add(Regex.Replace(name, "([a-z])([A-Z])", "$1 $2"));

            openButton.Click += OpenButtonOnClick;
            saveAllButton.Click += SaveAllButtonOnClick;
            filterComboBox.SelectedIndexChanged += FilterComboBoxOnSelectedIndexChanged;
            galleryManagerButton.Click += GalleryManagerButtonOnClick;
            coversButton.Click += CoversButtonOnClick;
            stopButton.Click += StopButtonOnClick;

            gridView.CellValueChanged += GridViewOnCellValueChanged;
            gridView.CellContentClick += GridViewOnCellContentClick;
            gridView.CellMouseUp += GridViewOnCellMouseUp;

            Closing += OnClosing;
        }

        public void ResetView(IEnumerable<MusicFile> displayedFiles, IEnumerable<MusicFile> selectedFiles)
        {
            filterComboBox.SelectedIndex = 0;
            RefreshGrid(displayedFiles, selectedFiles);
        }

        public void RefreshGrid(IEnumerable<MusicFile> displayedFiles, IEnumerable<MusicFile> selectedFiles)
        {
            statusStripLabel.Text = @"Refresh...";

            IEnumerable<MusicFile> selectedList = selectedFiles.ToList();
            gridView.Rows.Clear();

            foreach (MusicFile musicFile in displayedFiles)
            {
                gridView.Rows.Add(selectedList.Contains(musicFile), musicFile.Cover, Path.GetFileName(musicFile.Path),
                    musicFile.Album, "Play");
            }

            coversButton.Enabled = selectedList.Any();
        }

        public void HighlightAlbumChange(string filename)
        {
            foreach (DataGridViewRow row in gridView.Rows)
                if ((string)row.Cells["File"].Value == filename)
                    row.Cells["Album"].Style.ForeColor = Color.Red;
        }

        public void CompleteFolderChange()
        {
            statusStripLabel.Text = @"Load completed.";
        }

        public void CompleteSaveAll()
        {
            foreach (DataGridViewRow row in gridView.Rows)
                row.Cells["Album"].Style.ForeColor = Color.Black;

            Enabled = true;
            saveAllButton.Enabled = false;
            _isSaved = true;
            statusStripLabel.Text = @"Save completed.";
        }

        public void CompleteMusicPlay(string filename)
        {
            stopButton.Enabled = true;
            statusStripLabel.Text = @"Now playing : " + filename;
        }

        public void DisableStopButton()
        {
            stopButton.Enabled = false;
        }

        public void SetAsEdited()
        {
            _isSaved = false;
            saveAllButton.Enabled = true;
        }

        public void ShowCountsInStatusStrip(int entriesCount, int selectedCount, int displayedCount)
        {
            statusStripLabel.Text = string.Format("{0} entries, {1} selected, {2} displayed",
                entriesCount, selectedCount, displayedCount);

            coversButton.Enabled = selectedCount != 0;
        }

        private void OpenButtonOnClick(object sender, EventArgs eventArgs)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                statusStripLabel.Text = @"Loading files...";

                if (ChangeFolderRequest != null)
                    ChangeFolderRequest.Invoke(this, new ChangeFolderRequestArgs
                    {
                        FolderPath = dialog.SelectedPath
                    });
            }
        }

        private void SaveAllButtonOnClick(object sender, EventArgs eventArgs)
        {
            Enabled = false;

            if (SaveAllRequest != null)
                SaveAllRequest.Invoke(this, EventArgs.Empty);
        }

        private void FilterComboBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (ChangeFilterRequest == null)
                return;

            var args = new ChangeFilterRequestArgs
            {
                Filter = (MusicFileFilter)filterComboBox.SelectedIndex
            };

            ChangeFilterRequest.Invoke(this, args);
        }

        private void GalleryManagerButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (ShowGalleryManagerRequest != null)
                ShowGalleryManagerRequest.Invoke(this, EventArgs.Empty);
        }

        private void CoversButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (ShowCoverSeriesWizardRequest != null)
                ShowCoverSeriesWizardRequest.Invoke(this, EventArgs.Empty);
        }

        private void StopButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (StopMusicRequest != null)
                StopMusicRequest.Invoke(this, EventArgs.Empty);
        }

        private void GridViewOnCellValueChanged(object sender, DataGridViewCellEventArgs eventArgs)
        {
            DataGridViewRow row = gridView.Rows[eventArgs.RowIndex];

            if (row.Cells["File"].Value == null)
                return;

            switch (gridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "Selected":

                    var selectedArgs = new SelectedFileChangedArgs
                    {
                        Filename = (string)row.Cells["File"].Value,
                        IsSelected = (bool)row.Cells["Selected"].Value
                    };

                    if (SelectedFilesChanged != null)
                        SelectedFilesChanged.Invoke(this, selectedArgs);

                    break;
                case "Album":

                    var albumArgs = new AlbumNameChangedArgs
                    {
                        Filename = (string)row.Cells["File"].Value,
                        AlbumName = (string)row.Cells["Album"].Value
                    };

                    if (AlbumNameChanged != null)
                        AlbumNameChanged.Invoke(this, albumArgs);

                    break;
            }
        }

        private void GridViewOnCellContentClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            gridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

            switch (gridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "Song":

                    DataGridViewRow row = gridView.Rows[eventArgs.RowIndex];

                    var args = new PlayMusicArgs
                    {
                        Filename = (string)row.Cells["File"].Value
                    };

                    if (PlayMusicRequest != null)
                        PlayMusicRequest.Invoke(this, args);

                    break;
            }
        }

        private void GridViewOnCellMouseUp(object sender, DataGridViewCellMouseEventArgs eventArgs)
        {
            switch (gridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "Selected":
                    gridView.EndEdit();
                    break;
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            var args = new CloseRequestArgs();

            if (!_isSaved)
            {
                const string title = @"Confirmation";
                const string msg = @"Do you want to save modifications before quit ?";

                switch (MessageBox.Show(msg, title, MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        args.SaveRequested = true;
                        Enabled = false;
                        break;
                    case DialogResult.No:
                        args.SaveRequested = false;
                        Enabled = false;
                        break;
                    case DialogResult.Cancel:
                        cancelEventArgs.Cancel = true;
                        return;
                }
            }
            else
            {
                args.SaveRequested = false;
                Enabled = false;
            }

            if (CloseRequest != null)
                CloseRequest.Invoke(this, args);
        }

        public class ChangeFolderRequestArgs : EventArgs
        {
            public string FolderPath { get; set; }
        }

        public class ChangeFilterRequestArgs : EventArgs
        {
            public MusicFileFilter Filter { get; set; }
        }

        public class PlayMusicArgs : EventArgs
        {
            public string Filename { get; set; }
        }

        public class CloseRequestArgs : EventArgs
        {
            public bool SaveRequested { get; set; }
        }

        public class SelectedFileChangedArgs : EventArgs
        {
            public string Filename { get; set; }
            public bool IsSelected { get; set; }
        }

        public class AlbumNameChangedArgs : EventArgs
        {
            public string Filename { get; set; }
            public string AlbumName { get; set; }
        }
    }
}