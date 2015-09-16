using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoverMyOST.Galleries.Base
{
    public abstract class OnlineGallery : CoversGallery, IOnlineGallery
    {
        private readonly string _cacheSubFolder;

        private string CacheDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "CoverMyOST/", _cacheSubFolder);
            }
        }

        public bool UseCache { get; set; }

        protected OnlineGallery(string name, string cacheSubFolder)
        {
            _cacheSubFolder = cacheSubFolder;
            Name = name;

            Enable = true;
            UseCache = true;
        }

        public override CoverSearchResult Search(string query)
        {
            return SearchAsync(query, CancellationToken.None).Result;
        }

        public override async Task<CoverSearchResult> SearchAsync(string query, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (UseCache)
            {
                CoverEntry entry = SearchCached(query);
                if (entry != null)
                    return new CoverSearchResult{ entry };
            }

            await Task.Yield();
            ct.ThrowIfCancellationRequested();

            return await SearchOnlineAsync(query, ct);
        }

        public CoverSearchResult SearchOnline(string query)
        {
            return SearchOnlineAsync(query, CancellationToken.None).Result;
        }

        public abstract Task<CoverSearchResult> SearchOnlineAsync(string query, CancellationToken ct);

        public CoverEntry SearchCached(string query)
        {
            string filename = Path.Combine(CacheDirectory, query + ".png");

            if (!File.Exists(filename))
                return null;

            using (var streamReader = new StreamReader(filename))
            {
                return new CoverEntry(query, new Bitmap(streamReader.BaseStream), this);
            }
        }

        public void ClearCache()
        {
            if (Directory.Exists(CacheDirectory))
                Directory.Delete(CacheDirectory, true);
        }

        internal void AddCoverToCache(CoverEntry entry, string name)
        {
            if (!Directory.Exists(CacheDirectory))
                Directory.CreateDirectory(CacheDirectory);

            string path = Path.Combine(CacheDirectory, name + ".png");
            entry.Cover.Save(path, ImageFormat.Png);
        }
    }
}