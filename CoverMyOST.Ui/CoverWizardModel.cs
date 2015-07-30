using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace CoverMyOST.Ui
{
    public class CoverWizardModel
    {
        private readonly CoverSearchModel _coverSearch;
        private int _indexSelected;
        private MusicFileEditorModel _musicFileEditor;

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

        public event EventHandler<ProgressChangedEventArgs> SearchProgress
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

        public CoverWizardModel(CoverMyOSTClient client)
        {
            _musicFileEditor = new MusicFileEditorModel(client.AllSelectedFiles.ElementAt(0).Value);
            _coverSearch = new CoverSearchModel(client);
        }

        public void SetMusicFile(MusicFile musicFile)
        {
            if (_coverSearch.State != CoverSearchState.Wait)
                throw new InvalidOperationException("Can't set a MusicFile while another is processing or canceled.");

            _musicFileEditor.File = musicFile;
            _coverSearch.LaunchSearch(_musicFileEditor.EditedAlbum);
        }

        public void Close()
        {
            if (_coverSearch.State == CoverSearchState.Search)
                _coverSearch.CancelSearch();

            // BUG : Client continue to search if dialog close.
        }

        public void Apply()
        {
            if (_coverSearch.State == CoverSearchState.Cancel)
                throw new InvalidOperationException("Can't apply modifications while search is canceling.");

            if (_coverSearch.State == CoverSearchState.Search)
                _coverSearch.CancelSearch();
            ApplyToMusicFile();
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