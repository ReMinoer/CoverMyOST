using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoverMyOST.Galleries;

namespace CoverMyOST
{
    public class GalleryCollection : IEnumerable<ICoversGallery>
    {
        private readonly List<ICoversGallery> _list;
        private readonly List<LocalGallery> _local;
        public const string CacheRoot = "cache/";
        public MyAnimeListGallery MyAnimeList { get; private set; }

        public IReadOnlyList<ICoversGallery> List
        {
            get { return _list.AsReadOnly(); }
        }

        public IReadOnlyList<LocalGallery> Local
        {
            get { return _local.AsReadOnly(); }
        }

        public GalleryCollection()
        {
            MyAnimeList = new MyAnimeListGallery();

            _list = new List<ICoversGallery> {MyAnimeList};
            _local = new List<LocalGallery>();

            EnableAll();
        }

        public ICoversGallery this[string name]
        {
            get { return _list.First(gallery => gallery.Name == name); }
        }

        public IEnumerator<ICoversGallery> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public TOnlineGallery Get<TOnlineGallery>() where TOnlineGallery : OnlineGallery
        {
            return (_list.First(gallery => gallery is TOnlineGallery) as TOnlineGallery);
        }

        public void AddLocal(LocalGallery localGallery)
        {
            _local.Add(localGallery);
            _list.Add(localGallery);
        }

        public void AddLocal(string path)
        {
            AddLocal(new LocalGallery(path) {Enable = true});
        }

        public void ClearLocal()
        {
            foreach (LocalGallery localGallery in _local)
                _list.Remove(localGallery);

            _local.Clear();
        }

        public CoverSearchResult SearchCover(string query)
        {
            return SearchCoverAsync(query).Result;
        }

        public async Task<CoverSearchResult> SearchCoverAsync(string query)
        {
            var result = new CoverSearchResult();

            foreach (ICoversGallery gallery in _list)
                if (gallery.Enable)
                {
                    CoverSearchResult search = (gallery is OnlineGallery)
                        ? await (gallery as OnlineGallery).SearchAsync(query)
                        : gallery.Search(query);

                    foreach (CoverEntry entry in search)
                        result.Add(entry);
                }

            return result;
        }

        public CoverSearchResult SearchCover<TCoversGallery>(string query) where TCoversGallery : ICoversGallery
        {
            return SearchCoverAsync<TCoversGallery>(query).Result;
        }

        public async Task<CoverSearchResult> SearchCoverAsync<TCoversGallery>(string query)
            where TCoversGallery : ICoversGallery
        {
            if (typeof(TCoversGallery) == typeof(LocalGallery))
                return SearchLocalCover(query);

            foreach (ICoversGallery gallery in _list)
                if (gallery is TCoversGallery)
                    return await ((OnlineGallery)gallery).SearchAsync(query);

            return null;
        }

        public CoverSearchResult SearchCoverOnline<TOnlineGallery>(string query) where TOnlineGallery : OnlineGallery
        {
            return SearchCoverOnlineAsync<TOnlineGallery>(query).Result;
        }

        public async Task<CoverSearchResult> SearchCoverOnlineAsync<TOnlineGallery>(string query)
            where TOnlineGallery : OnlineGallery
        {
            foreach (ICoversGallery gallery in _list)
                if (gallery is TOnlineGallery)
                    return await (gallery as TOnlineGallery).SearchOnlineAsync(query);

            return null;
        }

        public CoverSearchResult SearchCoverCached(string query)
        {
            var result = new CoverSearchResult();

            foreach (ICoversGallery gallery in _list)
                if (gallery is OnlineGallery && gallery.Enable)
                    result.Add((gallery as OnlineGallery).SearchCached(query));

            return result;
        }

        public CoverEntry SearchCoverCached<TOnlineGallery>(string query) where TOnlineGallery : OnlineGallery
        {
            foreach (ICoversGallery gallery in _list)
                if (gallery is TOnlineGallery)
                    return (gallery as TOnlineGallery).SearchCached(query);

            return null;
        }

        public void ClearAllCache()
        {
            foreach (ICoversGallery gallery in _list)
                if (gallery is OnlineGallery)
                    (gallery as OnlineGallery).ClearCache();

            if (Directory.Exists(CacheRoot))
                Directory.Delete(CacheRoot, true);
        }

        public void EnableAll()
        {
            foreach (ICoversGallery gallery in _list)
                gallery.Enable = true;
        }

        public void DisableAll()
        {
            foreach (ICoversGallery gallery in _list)
                gallery.Enable = false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private CoverSearchResult SearchLocalCover(string query)
        {
            var result = new CoverSearchResult();

            foreach (ICoversGallery gallery in Local)
                if (gallery.Enable)
                    foreach (CoverEntry entry in gallery.Search(query))
                        result.Add(entry);

            return result;
        }
    }
}