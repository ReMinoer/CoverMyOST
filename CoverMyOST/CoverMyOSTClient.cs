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

        public void SaveAll()
        {
            foreach (MusicFile musicFile in _files.Values)
                musicFile.Save();
		}

		public Dictionary<string, Bitmap> SearchCover(string filePath)
		{
			return Galleries.SearchCover(Files[filePath].Album);
		}

		public Dictionary<string, Bitmap> SearchCover<TCoversGallery>(string filePath)
			where TCoversGallery : ICoversGallery
		{
			return Galleries.SearchCover<TCoversGallery>(Files[filePath].Album);
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