using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoverMyOST
{
    public class CoverSearchResult : IEnumerable<CoverEntry>
    {
        public CoverEntry this[string name] { get { return _list.First(cover => cover.Name == name); } }

        public int Count { get { return _list.Count; } }
        private readonly List<CoverEntry> _list = new List<CoverEntry>();

        public CoverSearchResult() {}

        public CoverSearchResult(params CoverEntry[] entries)
        {
            _list.AddRange(entries);
        }

        public IEnumerator<CoverEntry> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(CoverEntry coverEntry)
        {
            _list.Add(coverEntry);
        }

        public bool Contains(string name)
        {
            return _list.Any(cover => cover.Name == name);
        }
    }
}