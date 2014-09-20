using System.Drawing;
using System.IO;

namespace CoverMyOST
{
    public abstract class OnlineGallery : ICoversGallery
    {
        protected abstract string CacheDirectoryName { get; }

        public bool CacheEnable { get; set; }
        public abstract string Name { get; }
        public bool Enable { get; set; }

        public CoverSearchResult Search(string query)
        {
            if (CacheEnable)
            {
                CoverEntry entry = SearchCached(query);
                if (entry != null)
                    return new CoverSearchResult(entry);
            }

            return SearchOnline(query);
        }

        public abstract CoverSearchResult SearchOnline(string query);

        public CoverEntry SearchCached(string query)
        {
            string filename = Path.Combine(CacheDirectoryName + "/", query);
            return !File.Exists(filename) ? null : new CoverEntry(query, new Bitmap(filename), this);
        }

        internal void AddCoverToCache(CoverEntry entry, string name)
        {
            if (!Directory.Exists(CacheDirectoryName))
                Directory.CreateDirectory(CacheDirectoryName);

            string path = Path.Combine(CacheDirectoryName + "/", name);
            entry.Cover.Save(path);
        }

        public void ClearCache()
        {
            if (Directory.Exists(CacheDirectoryName))
                Directory.Delete(CacheDirectoryName, true);
        }
    }
}