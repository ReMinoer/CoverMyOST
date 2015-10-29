using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoverMyOST.Configuration;
using CoverMyOST.Editors;
using CoverMyOST.FileManagers;
using CoverMyOST.Galleries;
using NLog;

namespace CoverMyOST.Windows
{
    public class MainModel
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MusicFileLoader _musicFileLoader;
        private readonly MusicFileSelector _musicFileSelector;
        private readonly MusicFileRefiner _musicFileFilter;
        private readonly UserConfiguration _userConfiguration;
        public GalleryManager GalleryManager { get; private set; }
        public MusicFileCollectionEditor MusicFileCollectionEditor { get; private set; }

        public IEnumerable<MusicFile> Files
        {
            get { return _musicFileLoader.Files.Values; }
        }

        public IEnumerable<MusicFile> SelectedFiles
        {
            get { return _musicFileSelector.Files.Values; }
        }

        public IEnumerable<MusicFile> DisplayedFiles
        {
            get { return _musicFileFilter.Files.Values; }
        }

        public string WorkingDirectory
        {
            get { return _musicFileLoader.WorkingDirectory; }
        }

        public MainModel()
        {
            _musicFileLoader = new MusicFileLoader();
            _musicFileSelector = new MusicFileSelector(_musicFileLoader);
            _musicFileFilter = new MusicFileRefiner(_musicFileLoader);

            GalleryManager = new GalleryManager();
            MusicFileCollectionEditor = new MusicFileCollectionEditor();

            _userConfiguration = new UserConfiguration(_musicFileLoader, GalleryManager);

            try
            {
                Logger.Info("Load user configuration");
                _userConfiguration.Load();
            }
            catch (FileNotFoundException)
            {
            }

            ChangeAllFilesSelection(true);
            MusicFileCollectionEditor.Files = _musicFileLoader.Files.Values;
        }

        public MusicFile this[string filename]
        {
            get { return _musicFileLoader.Files[Path.Combine(WorkingDirectory, filename)]; }
        }

        public void ChangeFolder(string folderPath)
        {
            _musicFileLoader.ChangeDirectory(folderPath);
            Logger.Info("Load files from: {0}", WorkingDirectory);

            MusicFileCollectionEditor.Files = _musicFileLoader.Files.Values;
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
            _musicFileFilter.Filter = filter;
            Logger.Info("Displayed files: {0}/{1}", DisplayedFiles.Count(), Files.Count());
        }

        public void ChangeFileSelection(string path, bool isSelected)
        {
            if (isSelected)
            {
                Logger.Info("Select: {0}", Path.GetFileName(path));
                _musicFileSelector.Select(path);
            }
            else
            {
                Logger.Info("Unselect: {0}", Path.GetFileName(path));
                _musicFileSelector.Unselect(path);
            }
        }

        public void ChangeAllFilesSelection(bool isSelected)
        {
            if (isSelected)
            {
                Logger.Info("Select all files");
                _musicFileSelector.SelectAll();
            }
            else
            {
                Logger.Info("Unselect all files");
                _musicFileSelector.UnselectAll();
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