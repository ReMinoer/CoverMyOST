using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoverMyOST
{
    public class CoverSearchResult : IEnumerable<CoverSearchEntry>
    {
        public CoverSearchEntry this[string name] { get { return _list.First(cover => cover.Name == name); } }

        public int Count { get { return _list.Count; } }
        private readonly List<CoverSearchEntry> _list = new List<CoverSearchEntry>();

        public CoverSearchResult() {}

        public CoverSearchResult(params CoverSearchEntry[] entries)
        {
            _list.AddRange(entries);
        }

        public IEnumerator<CoverSearchEntry> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(CoverSearchEntry coverSearchEntry)
        {
            _list.Add(coverSearchEntry);
        }

        public bool Contains(string name)
        {
            return _list.Any(cover => cover.Name == name);
        }
    }
}