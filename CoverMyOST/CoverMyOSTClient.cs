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

        public string WorkingDirectory { get; private set; }

        public CoverMyOSTClient()
        {
            _files = new Dictionary<string, MusicFile>();
            WorkingDirectory = "";
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
    }
}