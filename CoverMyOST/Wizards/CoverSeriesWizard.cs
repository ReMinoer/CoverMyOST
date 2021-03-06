﻿using System;
using System.Drawing;
using System.Linq;
using CoverMyOST.Editors;
using CoverMyOST.Galleries;
using CoverMyOST.Searchers;
using NLog;

namespace CoverMyOST.Wizards
{
    // TODO : Handle connexion to Internet
    public class CoverSeriesWizard
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MusicFileCollectionEditor _musicFileEditors;
        private readonly CoverWizard _wizardModel;
        private bool _currentIsCancel;
        private bool _albumEdition;
        public int FileIndex { get; private set; }

        public int FilesCount
        {
            get { return _musicFileEditors.Count(); }
        }

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

        public event EventHandler Initialize;
        public event EventHandler ProcessEnd;

        public event EventHandler<SearchProgressEventArgs> SearchProgress
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

        public CoverSeriesWizard(MusicFileCollectionEditor musicFileEditors, GalleryManager galleryManager)
        {
            _musicFileEditors = musicFileEditors;
            _wizardModel = new CoverWizard(musicFileEditors.ElementAt(0), galleryManager);

            _wizardModel.SearchLaunch += WizardModelOnSearchLaunch;
            _wizardModel.SearchCancel += WizardModelOnSearchCancel;
            _wizardModel.SearchEnd += WizardModelOnSearchEnd;
        }

        public void ResetSearch()
        {
            _wizardModel.SetMusicFile(_musicFileEditors.ElementAt(FileIndex));
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
            _albumEdition = true;
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

        private void WizardModelOnSearchLaunch(object sender, EventArgs eventArgs)
        {
            _currentIsCancel = false;
            _albumEdition = false;

            if (Initialize != null)
                Initialize.Invoke(this, EventArgs.Empty);
        }

        private void WizardModelOnSearchCancel(object sender, EventArgs eventArgs)
        {
            _currentIsCancel = true;
        }

        private void WizardModelOnSearchEnd(object sender, EventArgs eventArgs)
        {
            if (FileIndex >= _musicFileEditors.Count() && ProcessEnd != null)
            {
                ProcessEnd.Invoke(this, EventArgs.Empty);
                return;
            }

            if (_currentIsCancel && (_albumEdition || NextFile()))
                ResetSearch();
        }

        private bool NextFile()
        {
            FileIndex++;
            if (FileIndex < _musicFileEditors.Count())
            {
                Logger.Info("Next file : {0}/{1}", FileIndex + 1, _musicFileEditors.Count());
                return true;
            }

            Logger.Info("No files left. Wizard will end");

            if (ProcessEnd != null)
                ProcessEnd.Invoke(this, EventArgs.Empty);

            return false;
        }
    }
}