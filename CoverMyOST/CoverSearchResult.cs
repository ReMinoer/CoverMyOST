using System.Collections.Generic;
using System.Linq;

namespace CoverMyOST
{
    public class CoverSearchResult : List<CoverEntry>
    {
        public CoverEntry this[string name]
        {
            get { return this.First(cover => cover.Name == name); }
        }

        public bool Contains(string name)
        {
            return this.Any(cover => cover.Name == name);
        }
    }
}