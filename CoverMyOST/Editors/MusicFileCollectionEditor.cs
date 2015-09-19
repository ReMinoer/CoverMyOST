using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoverMyOST.Editors
{
    public class MusicFileCollectionEditor : IEnumerable<MusicFileEditor>
    {
        private readonly Dictionary<string, MusicFileEditor> _editors;

        public IEnumerable<MusicFile> Files
        {
            get { return _editors.Values.Select(x => x.File); }
            set
            {
                _editors.Clear();
                foreach (MusicFile musicFile in value)
                    _editors.Add(musicFile.Path, new MusicFileEditor(musicFile));
            }
        }

        public MusicFileCollectionEditor()
        {
            _editors = new Dictionary<string, MusicFileEditor>();
        }

        public void ApplyAll()
        {
            foreach (MusicFileEditor editor in _editors.Values)
                editor.Apply();
        }

        public MusicFileEditor this[string path]
        {
            get { return _editors[path]; }
        }

        public IEnumerator<MusicFileEditor> GetEnumerator()
        {
            return _editors.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}