using System;
using System.IO;
using CoverMyOST.Models.Files.Base;
using TagLib;

namespace CoverMyOST.Models.Files
{
    public class MusicFilesLoader : MusicFilesContainer
    {
        public string WorkingDirectory { get; private set; }

        public MusicFilesLoader()
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