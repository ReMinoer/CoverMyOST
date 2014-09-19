using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CoverMyOST
{
    public class CoverMyOSTClient
    {
        public string WorkingDirectory { get; private set; }
        public GalleryCollection Galleries { get; private set; }

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return new ReadOnlyDictionary<string, MusicFile>(_filteredFiles); }
        }

        public MusicFileFilter Filter
        {
            get { return _filter; }
            set
            {
                FilterFiles(value);
                _filter = value;
            }
        }

        private readonly Dictionary<string, MusicFile> _allFiles;
        private MusicFileFilter _filter;
        private Dictionary<string, MusicFile> _filteredFiles;

        public CoverMyOSTClient()
        {
            WorkingDirectory = "";
            Galleries = new GalleryCollection();

            _allFiles = new Dictionary<string, MusicFile>();
            FilterFiles(MusicFileFilter.None);
        }

        public void ChangeDirectory(string path)
        {
            _allFiles.Clear();

            foreach (string file in Directory.EnumerateFiles(path))
            {
                string fullpath = Path.GetFullPath(file);
                _allFiles.Add(fullpath, new MusicFile(fullpath));
            }

            FilterFiles(MusicFileFilter.None);
            WorkingDirectory = path;
        }

        private void FilterFiles(MusicFileFilter filter)
        {
            switch (filter)
            {
                case MusicFileFilter.None:
                    _filteredFiles = _allFiles;
                    break;
                case MusicFileFilter.NoAlbum:
                    _filteredFiles = _allFiles.Where(e => string.IsNullOrEmpty(e.Value.Album)).
                                               ToDictionary(e => e.Key, e => e.Value);
                    break;
                case MusicFileFilter.NoCover:
                    _filteredFiles = _allFiles.Where(e => e.Value.Cover == null).ToDictionary(e => e.Key, e => e.Value);
                    break;
            }
        }

        public void SaveAll()
        {
            foreach (MusicFile musicFile in _allFiles.Values)
                musicFile.Save();
        }

        public Dictionary<string, Bitmap> SearchCover(string filePath)
        {
            return Galleries.SearchCover(_allFiles[filePath].Album);
        }

        public Dictionary<string, Bitmap> SearchCover<TCoversGallery>(string filePath)
            where TCoversGallery : ICoversGallery
        {
            return Galleries.SearchCover<TCoversGallery>(_allFiles[filePath].Album);
        }

        public Dictionary<string, Bitmap> SearchCover(MusicFile musicFile)
        {
            return SearchCover(musicFile.Path);
        }

        public Dictionary<string, Bitmap> SearchCover<TCoversGallery>(MusicFile musicFile)
            where TCoversGallery : ICoversGallery
        {
            return SearchCover<TCoversGallery>(musicFile.Path);
        }
    }
}