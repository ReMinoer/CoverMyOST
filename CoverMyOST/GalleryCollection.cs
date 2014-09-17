using System.Collections;
using System.Collections.Generic;
using CoverMyOST.Galleries;
using System.Drawing;
using System.Linq;

namespace CoverMyOST
{
    public class GalleryCollection : IEnumerable<ICoversGallery>
    {
        public IReadOnlyList<LocalGallery> Local { get { return _local.AsReadOnly(); } }
        public MyAnimeListGallery MyAnimeList { get; private set; }

        private readonly List<ICoversGallery> _list;
        private readonly List<LocalGallery> _local;

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

		public ICoversGallery this[int i]
		{
			get { return _list[i]; }
		}

        public IEnumerator<ICoversGallery> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddLocalGallery(string path)
        {
            var localGallery = new LocalGallery(path);
			localGallery.Enable = true;
            _local.Add(localGallery);
            _list.Add(localGallery);
		}

		public Dictionary<string, Bitmap> SearchCover(string query)
		{
			var result = new Dictionary<string, Bitmap>();

			foreach (ICoversGallery gallery in _list)
				if (gallery.Enable)
					foreach (var entry in gallery.Search(query))
						result.Add(entry.Key, entry.Value);

			return result;
		}

		public Dictionary<string, Bitmap> SearchCover<TCoversGallery>(string query)
			where TCoversGallery : ICoversGallery
		{
			if (typeof(TCoversGallery) == typeof(LocalGallery))
				return SearchLocalCover(query);

			foreach (ICoversGallery gallery in _list)
				if (gallery is TCoversGallery)
					return gallery.Search(query);

			return null;
		}

		private Dictionary<string, Bitmap> SearchLocalCover(string query)
		{
			var result = new Dictionary<string, Bitmap>();

			foreach (ICoversGallery gallery in Local)
				if (gallery.Enable)
					foreach (var entry in gallery.Search(query))
						result.Add(entry.Key, entry.Value);

			return result;
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