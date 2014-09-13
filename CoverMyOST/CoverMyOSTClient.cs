using System.Collections.Generic;
using System.IO;

namespace CoverMyOST
{
    public class CoverMyOSTClient
    {
        public Dictionary<string, MusicFile> Files { get; private set; }

        public CoverMyOSTClient()
        {
            Files = new Dictionary<string, MusicFile>();
        }

        public void AddFile(string path)
        {
            string fullpath = Path.GetFullPath(path);
            Files.Add(fullpath, new MusicFile(fullpath));
        }

        public void AddDirectory(string path, bool recursive = false)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (string file in Directory.EnumerateFiles(path, "*", searchOption))
                AddFile(file);
        }
    }
}