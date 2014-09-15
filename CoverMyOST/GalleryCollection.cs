using System.Collections;
using System.Collections.Generic;
using CoverMyOST.Galleries;

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
        }

		public void AddLocalGallery(string path)
		{
			var localGallery = new LocalGallery(path);
			_local.Add(localGallery);
			_list.Add(localGallery);
		}

        public IEnumerator<ICoversGallery> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}