using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoverMyOST.Galleries;

namespace CoverMyOST
{
    public class GalleryCollection : IEnumerable<ICoversGallery>
    {
        public IReadOnlyList<LocalGallery> Local { get { return _local.AsReadOnly(); } }
        public MyAnimeListGallery MyAnimeList { get; private set; }
        public ICoversGallery this[string name] { get { return _list.First(gallery => gallery.Name == name); } }

        private readonly List<ICoversGallery> _list;
        private readonly List<LocalGallery> _local;

        public const string CacheRoot = "cache/";

        public GalleryCollection()
        {
            MyAnimeList = new MyAnimeListGallery();

            _list = new List<ICoversGallery> {MyAnimeList};
            _local = new List<LocalGallery>();

            EnableAll();
        }

        public IEnumerator<ICoversGallery> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TOnlineGallery Get<TOnlineGallery>() where TOnlineGallery : OnlineGallery
        {
            return (_list.First(gallery => gallery is TOnlineGallery) as TOnlineGallery);
        }

        public void AddLocalGallery(string path)
        {
            var localGallery = new LocalGallery(path) {Enable = true};
            _local.Add(localGallery);
            _list.Add(localGallery);
        }

        public CoverSearchResult SearchCover(string query)
        {
            var result = new CoverSearchResult();

            foreach (ICoversGallery gallery in _list)
                if (gallery.Enable)
                    foreach (CoverEntry entry in gallery.Search(query))
                        result.Add(entry);

            return result;
        }

        public CoverSearchResult SearchCoverCached(string query)
        {
            var result = new CoverSearchResult();

            foreach (ICoversGallery gallery in _list)
                if (gallery is OnlineGallery && gallery.Enable)
                    result.Add((gallery as OnlineGallery).SearchCached(query));

            return result;
        }

        public CoverSearchResult SearchCover<TCoversGallery>(string query) where TCoversGallery : ICoversGallery
        {
            if (typeof(TCoversGallery) == typeof(LocalGallery))
                return SearchLocalCover(query);

            foreach (ICoversGallery gallery in _list)
                if (gallery is TCoversGallery)
                    return gallery.Search(query);

            return null;
        }

        public CoverSearchResult SearchCoverOnline<TOnlineGallery>(string query)
            where TOnlineGallery : OnlineGallery
        {
            foreach (ICoversGallery gallery in _list)
                if (gallery is TOnlineGallery)
                    return (gallery as TOnlineGallery).SearchOnline(query);

            return null;
        }

        public CoverEntry SearchCoverCached<TOnlineGallery>(string query)
            where TOnlineGallery : OnlineGallery
        {
            foreach (ICoversGallery gallery in _list)
                if (gallery is TOnlineGallery)
                    return (gallery as TOnlineGallery).SearchCached(query);

            return null;
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
    }
}