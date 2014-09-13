using System.Collections.Generic;

namespace CoverMyOST
{
    public class CoverMyOSTClient
    {
        public Dictionary<string, MusicFile> Files { get; private set; }

        public CoverMyOSTClient()
        {
            Files = new Dictionary<string, MusicFile>();
        }

        public void LoadFile(string path)
        {
            Files.Add(path, new MusicFile(path));
        }
    }
}