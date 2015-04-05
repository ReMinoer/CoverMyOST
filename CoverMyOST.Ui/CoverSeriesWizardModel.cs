﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace CoverMyOST.Ui
{
    public class CoverSeriesWizardModel
    {
        private readonly CoverMyOSTClient _client;
        private readonly CoverWizardModel _wizardModel;
        private bool _currentIsCancel;

        public int FileIndex { get; private set; }

        public int FilesCount
        {
            get { return _client.AllSelectedFiles.Count; }
        }

        public event EventHandler Initialize;
        public event EventHandler ProcessEnd;

        public CoverSearchState State
        {
            get { return _wizardModel.State; }
        }
        public string FilePath
        {
            get { return _wizardModel.FilePath; }
        }
        public Bitmap SelectedCover
        {
            get { return _wizardModel.SelectedCover; }
        }
        public string EditedAlbum
        {
            get { return _wizardModel.EditedAlbum; }
        }

        public event EventHandler<ProgressChangedEventArgs> SearchProgress
        {
            add { _wizardModel.SearchProgress += value; }
            remove { _wizardModel.SearchProgress -= value; }
        }
        public event EventHandler SearchCancel
        {
            add { _wizardModel.SearchCancel += value; }
            remove { _wizardModel.SearchCancel -= value; }
        }
        public event EventHandler<SearchErrorEventArgs> SearchError
        {
            add { _wizardModel.SearchError += value; }
            remove { _wizardModel.SearchError -= value; }
        }
        public event EventHandler SearchSuccess
        {
            add { _wizardModel.SearchSuccess += value; }
            remove { _wizardModel.SearchSuccess -= value; }
        }
        public event EventHandler SearchEnd
        {
            add { _wizardModel.SearchEnd += value; }
            remove { _wizardModel.SearchEnd -= value; }
        }

        public CoverSeriesWizardModel(CoverMyOSTClient client)
        {
            _client = client;
            _wizardModel = new CoverWizardModel(client);

            _wizardModel.SearchLaunch += WizardModelOnSearchLaunch;
            _wizardModel.SearchCancel += WizardModelOnSearchCancel;
            _wizardModel.SearchEnd += WizardModelOnSearchEnd;
        }

        public void ResetSearch()
        {
            _wizardModel.SetMusicFile(_client.AllSelectedFiles.ElementAt(FileIndex).Value);
            _currentIsCancel = false;

            if (Initialize != null)
                Initialize.Invoke(this, EventArgs.Empty);
        }

        public void Close()
        {
            _wizardModel.Close();
        }

        public void Apply()
        {
            if (_wizardModel.State == CoverSearchState.Cancel)
                throw new InvalidOperationException("Can't apply modifications while search is canceling.");

            switch (_wizardModel.State)
            {
                case CoverSearchState.Search:
                    _wizardModel.Apply();
                    NextFile();
                    break;
                case CoverSearchState.Wait:
                    _wizardModel.Apply();
                    if (NextFile())
                        ResetSearch();
                    break;
            }
        }

        public void EditAlbum(string albumName)
        {
            _wizardModel.EditAlbum(albumName);
        }

        public void ChangeCoverSelected(int index)
        {
            _wizardModel.ChangeCoverSelected(index);
        }

        public void ResetCoverSelected()
        {
            _wizardModel.ResetCoverSelected();
        }

        public void RemoveCoverSelected()
        {
            _wizardModel.RemoveCoverSelected();
        }

        private bool NextFile()
        {
            FileIndex++;
            if (FileIndex < _client.AllSelectedFiles.Count)
                return true;

            if (ProcessEnd != null)
                ProcessEnd.Invoke(this, EventArgs.Empty);

            return false;
        }

        private void WizardModelOnSearchLaunch(object sender, EventArgs eventArgs)
        {
            if (Initialize != null)
                Initialize.Invoke(this, EventArgs.Empty);
        }

        private void WizardModelOnSearchCancel(object sender, EventArgs eventArgs)
        {
            _currentIsCancel = true;
        }

        private void WizardModelOnSearchEnd(object sender, EventArgs eventArgs)
        {
            if (FileIndex >= _client.AllSelectedFiles.Count && ProcessEnd != null)
            {
                ProcessEnd.Invoke(this, EventArgs.Empty);
                return;
            }

            if (_currentIsCancel)
                ResetSearch();
        }
    }
}