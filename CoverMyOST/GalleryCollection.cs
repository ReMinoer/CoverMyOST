using System.Collections;
using System.Collections.Generic;
using CoverMyOST.Galleries;

namespace CoverMyOST
{
    public class GalleryCollection : IEnumerable<ICoversGallery>
    {
        public MyAnimeListGallery MyAnimeList { get; private set; }

        private readonly List<ICoversGallery> _list;

        public GalleryCollection()
        {
            MyAnimeList = new MyAnimeListGallery();

            _list = new List<ICoversGallery> {MyAnimeList};
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