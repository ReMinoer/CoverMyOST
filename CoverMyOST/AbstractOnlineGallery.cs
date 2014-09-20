using System.Drawing;
using System.IO;

namespace CoverMyOST
{
    public abstract class AbstractOnlineGallery : ICoversGallery
    {
        protected abstract string CacheDirectoryName { get; }

        public bool CacheEnable { get; set; }
        public abstract string Name { get; }
        public bool Enable { get; set; }

        public CoverSearchResult Search(string query)
        {
            if (CacheEnable)
            {
                CoverSearchEntry entry = SearchInCache(query);
                if (entry != null)
                    return new CoverSearchResult(entry);
            }

            return SearchOnline(query);
        }

        public abstract CoverSearchResult SearchOnline(string query);

        public CoverSearchEntry SearchInCache(string query)
        {
            string filename = Path.Combine(CacheDirectoryName + "/", query);
            return !File.Exists(filename) ? null : new CoverSearchEntry(query, new Bitmap(filename), this);
        }

        internal void AddCoverToCache(CoverSearchEntry entry, string name)
        {
            if (!Directory.Exists(CacheDirectoryName))
                Directory.CreateDirectory(CacheDirectoryName);

            string path = Path.Combine(CacheDirectoryName + "/", name);
            entry.Cover.Save(path);
        }

        public void ClearCache()
        {
            Directory.Delete(CacheDirectoryName, true);
        }
    }
}