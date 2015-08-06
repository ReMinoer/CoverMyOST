using System;
using System.Collections.Generic;
using System.Linq;
using CoverMyOST.Models.Files.Base;

namespace CoverMyOST.Models.Files
{
    public class MusicFilesSelector : MusicFilesContainerDecorator
    {
        private readonly List<string> _selectedFilesName;

        public MusicFilesSelector()
        {
            _selectedFilesName = new List<string>();
        }

        public void Select(string filename)
        {
            Refresh();

            if (!Component.Files.ContainsKey(filename))
                throw new ArgumentException("Component doesn't contain the filename " + filename);

            if (!_selectedFilesName.Contains(filename))
                _selectedFilesName.Add(filename);
        }

        public void Unselect(string filename)
        {
            Refresh();

            if (!Component.Files.ContainsKey(filename))
                throw new ArgumentException("Component doesn't contain the filename " + filename);

            if (_selectedFilesName.Contains(filename))
                _selectedFilesName.Add(filename);
        }

        public void SelectAll()
        {
            Refresh();
            _selectedFilesName.Clear();

            foreach (string filename in Component.Files.Keys)
                _selectedFilesName.Add(filename);
        }

        public void UnselectAll()
        {
            _selectedFilesName.Clear();
        }

        protected override void Refresh(IReadOnlyDictionary<string, MusicFile> componentFiles)
        {
            foreach (string filename in _selectedFilesName.Where(filename => !componentFiles.ContainsKey(filename)))
                _selectedFilesName.Remove(filename);
        }
    }
}