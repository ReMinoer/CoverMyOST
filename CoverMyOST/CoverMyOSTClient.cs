using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace CoverMyOST
{
    public class CoverMyOSTClient
    {
        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return new ReadOnlyDictionary<string, MusicFile>(_files); }
        }
        private readonly Dictionary<string, MusicFile> _files;

        public CoverMyOSTClient()
        {
            _files = new Dictionary<string, MusicFile>();
        }

        public void AddFile(string path)
        {
            string fullpath = Path.GetFullPath(path);
            _files.Add(fullpath, new MusicFile(fullpath));
        }

        public void AddDirectory(string path, bool recursive = false)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (string file in Directory.EnumerateFiles(path, "*", searchOption))
                AddFile(file);
        }
    }
}