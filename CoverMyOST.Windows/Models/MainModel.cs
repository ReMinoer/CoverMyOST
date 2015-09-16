using System.Collections.Generic;
using System.IO;
using CoverMyOST.Models.Configuration;
using CoverMyOST.Models.Edition;
using CoverMyOST.Models.Files;
using CoverMyOST.Models.Galleries;

namespace CoverMyOST.Windows.Models
{
    public class MainModel
    {
        private readonly MusicFilesFilter _musicFilesFilter;
        private readonly MusicFilesLoader _musicFilesLoader;
        private readonly MusicFilesSelector _musicFilesSelector;
        private readonly UserConfiguration _userConfiguration;
        public GalleryManager GalleryManager { get; private set; }
        public MusicFileCollectionEditor MusicFileCollectionEditor { get; private set; }

        public IEnumerable<MusicFile> Files
        {
            get { return _musicFilesLoader.Files.Values; }
        }

        public IEnumerable<MusicFile> SelectedFiles
        {
            get { return _musicFilesSelector.Files.Values; }
        }

        public IEnumerable<MusicFile> DisplayedFiles
        {
            get { return _musicFilesFilter.Files.Values; }
        }

        public string WorkingDirectory
        {
            get { return _musicFilesLoader.WorkingDirectory; }
        }

        public MainModel()
        {
            _musicFilesLoader = new MusicFilesLoader();
            _musicFilesSelector = new MusicFilesSelector(_musicFilesLoader);
            _musicFilesFilter = new MusicFilesFilter(_musicFilesLoader);

            GalleryManager = new GalleryManager();
            MusicFileCollectionEditor = new MusicFileCollectionEditor();

            _userConfiguration = new UserConfiguration(_musicFilesLoader, GalleryManager);

            try
            {
                _userConfiguration.Load();
            }
            catch (FileNotFoundException)
            {
            }

            _musicFilesSelector.SelectAll();
            MusicFileCollectionEditor.Files = _musicFilesLoader.Files.Values;
        }

        public void ChangeFolder(string folderPath)
        {
            _musicFilesLoader.ChangeDirectory(folderPath);
            MusicFileCollectionEditor.Files = _musicFilesLoader.Files.Values;
            _userConfiguration.Save();
        }

        public void SaveAll()
        {
            MusicFileCollectionEditor.ApplyAll();
        }

        public void SaveConfiguration()
        {
            _userConfiguration.Save();
        }

        public void ChangeFilter(MusicFileFilter musicFileFilter)
        {
            _musicFilesFilter.Filter = musicFileFilter;
        }

        public void ChangeFileSelection(string path, bool isSelected)
        {
            if (isSelected)
                _musicFilesSelector.Select(path);
            else
                _musicFilesSelector.Unselect(path);
        }

        public void ChangeFileAlbumName(string path, string albumName)
        {
            MusicFileEditor editor = MusicFileCollectionEditor[path];

            if (editor.EditedAlbum != albumName)
                editor.EditedAlbum = albumName;
        }
    }
}