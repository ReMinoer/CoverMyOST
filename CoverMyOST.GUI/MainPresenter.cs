using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CoverMyOST.GUI.Dialogs;

namespace CoverMyOST.GUI
{
    // TODO : Create the dialog for galleries options
    // TODO : Fix save conditions & signs of edition
    // TODO : Update button state for stop music
    public class MainPresenter
    {
        private readonly CoverMyOSTClient _client;
        private readonly IMainView _view;

        private bool _isSaved = true;

        public MainPresenter(IMainView view)
        {
            _view = view;

            _client = new CoverMyOSTClient();
            _client.LoadConfiguration();

            foreach (string name in Enum.GetNames(typeof(MusicFileFilter)))
                _view.FilterComboBox.Items.Add(Regex.Replace(name, "([a-z])([A-Z])", "$1 $2"));
            _view.FilterComboBox.SelectedIndex = 0;

            _view.OpenButton.Click += OpenButtonOnClick;
            _view.SaveAllButton.Click += SaveAllButtonOnClick;
            _view.FilterComboBox.SelectedIndexChanged += FilterComboBoxOnSelectedIndexChanged;
            _view.GalleryManagerButton.Click += GalleryManagerButtonOnClick;
            _view.StopButton.Click += StopButtonOnClick;
            _view.CoversButton.Click += CoversButtonOnClick;

            _view.GridView.CellContentClick += GridViewOnCellContentClick;
            _view.GridView.CellValueChanged += GridViewOnCellValueChanged;
            _view.GridView.CellMouseUp += GridViewOnCellMouseUp;

            _view.ClosingProgram += ViewOnClosing;

            RefreshGrid();
        }

        private void GridViewOnCellValueChanged(object sender, DataGridViewCellEventArgs eventArgs)
        {
            DataGridViewRow row = _view.GridView.Rows[eventArgs.RowIndex];

			if (row.Cells["File"].Value == null)
				return;

            string path = Path.Combine(_client.WorkingDirectory, (string)row.Cells["File"].Value);

            switch (_view.GridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "Selected":
                    _client.Files[path].Selected = (bool)row.Cells["Selected"].Value;
                    ShowCountsInStatusStrip();
                    break;
                case "Album":
                    if (_client.Files[path].Album != (string)row.Cells["Album"].Value)
                    {
                        _client.Files[path].Album = (string)row.Cells["Album"].Value;
                        row.Cells["Album"].Style.ForeColor = Color.Red;
                        OnModification();
                    }
                    break;
            }
        }

        private void GridViewOnCellContentClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            _view.GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

            DataGridViewRow row = _view.GridView.Rows[eventArgs.RowIndex];
            var name = (string)row.Cells["File"].Value;
            string path = Path.Combine(_client.WorkingDirectory, name);

            switch (_view.GridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "Song":
                    _client.PlayMusic(path);
                    _view.StatusStripLabel = @"Now playing : " + name;
                    break;
            }
        }

        private void GridViewOnCellMouseUp(object sender, DataGridViewCellMouseEventArgs eventArgs)
        {
            switch (_view.GridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "Selected":
                    _view.GridView.EndEdit();
                    break;
            }
        }

        private void RefreshGrid()
        {
            _view.StatusStripLabel = @"Refresh...";

            _view.GridView.Rows.Clear();

            foreach (MusicFileEntry musicFile in _client.Files.Values)
                _view.GridView.Rows.Add(musicFile.Selected, musicFile.Cover, Path.GetFileName(musicFile.Path),
                    musicFile.Album, "Play");

            ShowCountsInStatusStrip();
        }

        private void OpenButtonOnClick(object sender, EventArgs eventArgs)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _view.StatusStripLabel = @"Loading files...";
                _client.ChangeDirectory(dialog.SelectedPath);
                _view.StatusStripLabel = @"Load completed.";

                _view.FilterComboBox.SelectedIndex = 0;
                _client.StopMusic();
                _client.SaveConfiguration();
                RefreshGrid();
            }
        }

        private void SaveAllButtonOnClick(object sender, EventArgs eventArgs)
        {
            _view.Enabled = false;

            _client.SaveAll();
            _client.SaveConfiguration();

            foreach (DataGridViewRow row in _view.GridView.Rows)
                row.Cells["Album"].Style.ForeColor = Color.Black;

            _isSaved = true;
            _view.SaveAllButton.Enabled = false;

            _view.Enabled = true;
            _view.StatusStripLabel = @"Save completed.";
        }

        private void FilterComboBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            _client.Filter = (MusicFileFilter)_view.FilterComboBox.SelectedIndex;
            ShowCountsInStatusStrip();
            RefreshGrid();
        }

        private void GalleryManagerButtonOnClick(object sender, EventArgs eventArgs)
        {
            var galleryManagerView = new GalleryManagerView(_client);
            galleryManagerView.ShowDialog();
        }

        private void StopButtonOnClick(object sender, EventArgs eventArgs)
        {
            _client.StopMusic();
            ShowCountsInStatusStrip();
        }

        private void CoversButtonOnClick(object sender, EventArgs eventArgs)
        {
            _client.StopMusic();

            var coverSearchView = new CoverSearchView(_client);
            coverSearchView.ShowDialog();
            RefreshGrid();
            OnModification();
        }

        private void OnModification()
        {
            _isSaved = false;
            _view.SaveAllButton.Enabled = true;
        }

        private void ShowCountsInStatusStrip()
        {
            _view.StatusStripLabel = string.Format("{0} entries, {1} selected, {2} displayed", _client.AllFiles.Count,
                _client.AllSelectedFiles.Count, _client.Files.Count);

            _view.CoversButton.Enabled = _client.AllSelectedFiles.Count != 0;
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (_isSaved)
                return;

            const string title = @"Confirmation";
            const string msg = @"Do you want to save modifications before quit ?";

            switch (MessageBox.Show(msg, title, MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    _view.Enabled = false;
                    _client.SaveAll();
                    _client.SaveConfiguration();
                    break;
                case DialogResult.No:
                    _view.Enabled = false;
                    _client.SaveConfiguration();
                    break;
                case DialogResult.Cancel:
                    cancelEventArgs.Cancel = true;
                    break;
            }
        }
    }
}