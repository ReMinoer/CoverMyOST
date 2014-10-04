using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace CoverMyOST
{
    public abstract class OnlineGallery : ICoversGallery
    {
        public bool CacheEnable { get; set; }

        protected abstract string CacheDirectoryName { get; }

        private string CacheDirectory
        {
            get { return Path.Combine(GalleryCollection.CacheRoot, CacheDirectoryName + "/"); }
        }
        public abstract string Name { get; }
        public bool Enable { get; set; }

        public CoverSearchResult Search(string query)
        {
            return SearchAsync(query).Result;
        }

        public async Task<CoverSearchResult> SearchAsync(string query)
        {
            if (CacheEnable)
            {
                CoverEntry entry = SearchCached(query);
                if (entry != null)
                    return new CoverSearchResult(entry);
            }

            return await SearchOnlineAsync(query);
        }

        public CoverSearchResult SearchOnline(string query)
        {
            return SearchOnlineAsync(query).Result;
        }

        public abstract Task<CoverSearchResult> SearchOnlineAsync(string query);

        public CoverEntry SearchCached(string query)
        {
            string filename = Path.Combine(CacheDirectory, query + ".png");
            return !File.Exists(filename) ? null : new CoverEntry(query, new Bitmap(filename), this);
        }

        internal void AddCoverToCache(CoverEntry entry, string name)
        {
            if (!Directory.Exists(CacheDirectory))
                Directory.CreateDirectory(CacheDirectory);

            string path = Path.Combine(CacheDirectory, name + ".png");
            entry.Cover.Save(path, ImageFormat.Png);
        }

        public void ClearCache()
        {
            if (Directory.Exists(CacheDirectory))
                Directory.Delete(CacheDirectory, true);
        }
    }
}