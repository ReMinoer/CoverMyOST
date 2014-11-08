using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace CoverMyOST.GUI
{
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

            _view.FilterComboBox.SelectedIndex = 0;

            _view.OpenButton.Click += OpenButtonOnClick;
            _view.SaveAllButton.Click += SaveAllButtonOnClick;
            _view.FilterComboBox.SelectedIndexChanged += FilterComboBoxOnSelectedIndexChanged;

            _view.ClosingProgram += ViewOnClosing;

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            _view.StatusStripLabel = @"Refresh...";

            _view.GridView.Rows.Clear();

            foreach (MusicFile musicFile in _client.Files.Values)
                _view.GridView.Rows.Add(musicFile.Cover, Path.GetFileName(musicFile.Path), musicFile.Album);

            _view.StatusStripLabel = "";
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
                RefreshGrid();
            }
        }

        private void SaveAllButtonOnClick(object sender, EventArgs eventArgs)
        {
            _client.SaveAll();
            _isSaved = true;
            _view.StatusStripLabel = @"Save completed.";
        }

        private void FilterComboBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            switch (_view.FilterComboBox.SelectedIndex)
            {
                case 0:
                    _client.Filter = MusicFileFilter.None;
                    break;
                case 1:
                    _client.Filter = MusicFileFilter.NoAlbum;
                    break;
                case 2:
                    _client.Filter = MusicFileFilter.NoCover;
                    break;
            }

            RefreshGrid();
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
                    _client.SaveAll();
                    _client.SaveConfiguration();
                    break;
                case DialogResult.No:
                    _client.SaveConfiguration();
                    break;
                case DialogResult.Cancel:
                    cancelEventArgs.Cancel = true;
                    break;
            }
        }
    }
}