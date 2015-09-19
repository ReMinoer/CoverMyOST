using System;
using System.Collections.Generic;
using System.Linq;
using CoverMyOST.FileManagers.Base;

namespace CoverMyOST.FileManagers
{
    public class MusicFileSelector : MusicFileManager
    {
        public MusicFileSelector(IMusicFileManager parent)
        {
            Parent = parent;
        }

        public void Select(string filename)
        {
            Refresh();

            if (!Parent.Files.ContainsKey(filename))
                throw new ArgumentException("Component doesn't contain the filename " + filename);

            if (!LocalFiles.ContainsKey(filename))
                LocalFiles.Add(filename, Parent.Files[filename]);
        }

        public void Unselect(string filename)
        {
            Refresh();

            if (!Parent.Files.ContainsKey(filename))
                throw new ArgumentException("Component doesn't contain the filename " + filename);

            if (LocalFiles.ContainsKey(filename))
                LocalFiles.Remove(filename);
        }

        public void SelectAll()
        {
            Refresh();
            LocalFiles.Clear();

            foreach (KeyValuePair<string, MusicFile> pair in Parent.Files)
                LocalFiles.Add(pair.Key, pair.Value);
        }

        public void UnselectAll()
        {
            LocalFiles.Clear();
        }

        protected override void RefreshLocal()
        {
            var toRemove = new List<string>();

            foreach (string filename in LocalFiles.Where(file => !Parent.Files.ContainsKey(file.Key)).Select(x => x.Key))
                toRemove.Add(filename);

            foreach (string s in toRemove)
                LocalFiles.Remove(s);
        }
    }
}