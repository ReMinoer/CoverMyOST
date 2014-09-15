using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

namespace CoverMyOST
{
    public class CoverMyOSTClient
    {
        public string WorkingDirectory { get; private set; }
        public GalleryCollection Galleries { get; set; }

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return new ReadOnlyDictionary<string, MusicFile>(_files); }
        }
        private readonly Dictionary<string, MusicFile> _files;

        public CoverMyOSTClient()
        {
            WorkingDirectory = "";
            Galleries = new GalleryCollection();
            _files = new Dictionary<string, MusicFile>();
        }

        public void ChangeDirectory(string path)
        {
            foreach (string file in Directory.EnumerateFiles(path))
            {
                string fullpath = Path.GetFullPath(file);
                _files.Add(fullpath, new MusicFile(fullpath));
            }

            WorkingDirectory = path;
        }

        public void AddLocalGallery(string path)
        {
            Galleries.AddLocalGallery(path);
        }

        public void SaveAll()
        {
            foreach (MusicFile musicFile in _files.Values)
                musicFile.Save();
        }

        public Dictionary<string, Bitmap> SearchCover(string filePath)
        {
            var result = new Dictionary<string, Bitmap>();

            foreach (ICoversGallery gallery in Galleries)
                foreach (var entry in gallery.Search(Files[filePath].Album))
                    result.Add(entry.Key, entry.Value);

            return result;
        }

        public Dictionary<string, Bitmap> SearchCover<TCoversGallery>(string filePath)
            where TCoversGallery : ICoversGallery
        {
            if (typeof(TCoversGallery) == typeof(LocalGallery))
                return SearchLocalCover(filePath);

            foreach (ICoversGallery gallery in Galleries)
                if (gallery is TCoversGallery)
                    return gallery.Search(Files[filePath].Album);

            return null;
        }

        private Dictionary<string, Bitmap> SearchLocalCover(string filePath)
        {
            var result = new Dictionary<string, Bitmap>();

            foreach (ICoversGallery gallery in Galleries)
                if (gallery is LocalGallery)
                    foreach (var entry in gallery.Search(Files[filePath].Album))
                        result.Add(entry.Key, entry.Value);

            return result;
        }
    }
}