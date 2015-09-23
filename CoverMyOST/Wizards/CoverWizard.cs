using System;
using System.Drawing;
using System.IO;
using System.Linq;
using CoverMyOST.Editors;
using CoverMyOST.Galleries;
using CoverMyOST.Searchers;
using NLog;

namespace CoverMyOST.Wizards
{
    public class CoverWizard
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly CoverSearcher _coverSearch;
        private MusicFileEditor _musicFileEditor;
        private int _indexSelected;

        public CoverSearchState State
        {
            get { return _coverSearch.State; }
        }

        public string FilePath
        {
            get { return _musicFileEditor.File.Path; }
        }

        public Bitmap SelectedCover
        {
            get { return _musicFileEditor.SelectedCover; }
        }

        public string EditedAlbum
        {
            get { return _musicFileEditor.EditedAlbum; }
        }

        public event EventHandler SearchLaunch
        {
            add { _coverSearch.SearchLaunch += value; }
            remove { _coverSearch.SearchLaunch -= value; }
        }

        public event EventHandler<SearchProgressEventArgs> SearchProgress
        {
            add { _coverSearch.SearchProgress += value; }
            remove { _coverSearch.SearchProgress -= value; }
        }

        public event EventHandler SearchCancel
        {
            add { _coverSearch.SearchCancel += value; }
            remove { _coverSearch.SearchCancel -= value; }
        }

        public event EventHandler<SearchErrorEventArgs> SearchError
        {
            add { _coverSearch.SearchError += value; }
            remove { _coverSearch.SearchError -= value; }
        }

        public event EventHandler SearchSuccess
        {
            add { _coverSearch.SearchSuccess += value; }
            remove { _coverSearch.SearchSuccess -= value; }
        }

        public event EventHandler SearchEnd
        {
            add { _coverSearch.SearchEnd += value; }
            remove { _coverSearch.SearchEnd -= value; }
        }

        public CoverWizard(MusicFileEditor musicFileEditor, GalleryManager galleryManager)
        {
            _musicFileEditor = musicFileEditor;
            _coverSearch = new CoverSearcher(galleryManager);
            _indexSelected = -1;
        }

        public void SetMusicFile(MusicFileEditor musicFileEditor)
        {
            if (_coverSearch.State != CoverSearchState.Wait)
                throw new InvalidOperationException("Can't set a MusicFile while another is processing or canceled.");

            _musicFileEditor = musicFileEditor;
            _coverSearch.LaunchSearch(_musicFileEditor.EditedAlbum);
        }

        public void Close()
        {
            if (_coverSearch.State == CoverSearchState.Search)
                _coverSearch.CancelSearch();
        }

        public void Apply()
        {
            if (_coverSearch.State == CoverSearchState.Cancel)
                throw new InvalidOperationException("Can't apply modifications while search is canceling.");

            Logger.Info("Apply changes to: {0}", Path.GetFileName(FilePath));
            ApplyToMusicFile();
            _indexSelected = -1;

            if (_coverSearch.State == CoverSearchState.Search)
                _coverSearch.CancelSearch();
        }

        public void EditAlbum(string albumName)
        {
            if (_musicFileEditor.EditedAlbum == albumName)
                return;

            _musicFileEditor.EditedAlbum = albumName;

            switch (_coverSearch.State)
            {
                case CoverSearchState.Search:
                    _coverSearch.CancelSearch();
                    break;
                case CoverSearchState.Wait:
                    _coverSearch.LaunchSearch(_musicFileEditor.EditedAlbum);
                    break;
            }
        }

        public void ChangeCoverSelected(int index)
        {
            if (_coverSearch.State == CoverSearchState.Cancel)
                return;

            _musicFileEditor.SelectedCover = _coverSearch.SearchResults.ElementAt(index).Cover;
            _indexSelected = index;
        }

        public void ResetCoverSelected()
        {
            if (_coverSearch.State == CoverSearchState.Cancel)
                return;

            _musicFileEditor.ResetCover();
            _indexSelected = -1;
        }

        public void RemoveCoverSelected()
        {
            if (_coverSearch.State == CoverSearchState.Cancel)
                return;

            _musicFileEditor.SelectedCover = null;
            _indexSelected = -1;
        }

        private void ApplyToMusicFile()
        {
            _musicFileEditor.Apply();
            if (_indexSelected != -1)
            {
                CoverEntry entry = _coverSearch.SearchResults[_indexSelected];
                entry.AddToGalleryCache(_musicFileEditor.File.Album);
            }
        }
    }
}