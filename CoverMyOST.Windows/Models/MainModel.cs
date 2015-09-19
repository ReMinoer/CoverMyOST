using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoverMyOST.Models.Configuration;
using CoverMyOST.Models.Edition;
using CoverMyOST.Models.Files;
using CoverMyOST.Models.Galleries;
using NLog;

namespace CoverMyOST.Windows.Models
{
    public class MainModel
    {
        private readonly MusicFilesFilter _musicFilesFilter;
        private readonly MusicFilesLoader _musicFilesLoader;
        private readonly MusicFilesSelector _musicFilesSelector;
        private readonly UserConfiguration _userConfiguration;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
                Logger.Info("Load user configuration");
                _userConfiguration.Load();
            }
            catch (FileNotFoundException)
            {
            }

            ChangeAllFilesSelection(true);
            MusicFileCollectionEditor.Files = _musicFilesLoader.Files.Values;
        }

        public void ChangeFolder(string folderPath)
        {
            _musicFilesLoader.ChangeDirectory(folderPath);
            Logger.Info("Load files from: {0}", WorkingDirectory);

            MusicFileCollectionEditor.Files = _musicFilesLoader.Files.Values;
            _userConfiguration.Save();
        }

        public void SaveAll()
        {
            Logger.Info("Apply changes to all files");
            MusicFileCollectionEditor.ApplyAll();
        }

        public void SaveConfiguration()
        {
            Logger.Info("Save user configuration");
            _userConfiguration.Save();
        }

        public void ChangeFilter(MusicFileFilter filter)
        {
            Logger.Info("Filter with: {0}", filter);
            _musicFilesFilter.Filter = filter;
            Logger.Info("Displayed files: {0}/{1}", DisplayedFiles.Count(), Files.Count());
        }

        public void ChangeFileSelection(string path, bool isSelected)
        {
            if (isSelected)
            {
                Logger.Info("Select: {0}", Path.GetFileName(path));
                _musicFilesSelector.Select(path);
            }
            else
            {
                Logger.Info("Unselect: {0}", Path.GetFileName(path));
                _musicFilesSelector.Unselect(path);
            }
        }

        public void ChangeAllFilesSelection(bool isSelected)
        {
            if (isSelected)
            {
                Logger.Info("Select all files");
                _musicFilesSelector.SelectAll();
            }
            else
            {
                Logger.Info("Unselect all files");
                _musicFilesSelector.UnselectAll();
            }
        }

        public void ChangeFileAlbumName(string path, string albumName)
        {
            MusicFileEditor editor = MusicFileCollectionEditor[path];

            if (editor.EditedAlbum != albumName)
                editor.EditedAlbum = albumName;
        }
    }
}