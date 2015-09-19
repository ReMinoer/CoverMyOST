using System;
using System.IO;
using CoverMyOST.FileManagers.Base;
using TagLib;

namespace CoverMyOST.FileManagers
{
    public class MusicFileLoader : MusicFileManager
    {
        public string WorkingDirectory { get; private set; }

        public MusicFileLoader()
        {
            WorkingDirectory = "";
        }

        public void ChangeDirectory(string path)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException("Directory specify does not exist.");

            WorkingDirectory = path;

            Refresh();
        }

        protected override void RefreshLocal()
        {
            LocalFiles.Clear();

            foreach (string file in Directory.EnumerateFiles(WorkingDirectory))
            {
                string fullpath = Path.GetFullPath(file);
                try
                {
                    var musicFile = new MusicFile(fullpath);
                    LocalFiles.Add(fullpath, musicFile);
                }
                catch (UnsupportedFormatException)
                {
                }
            }
        }
    }
}