using System.Drawing;
using CoverMyOST.Galleries.Base;

namespace CoverMyOST
{
    public class CoverEntry
    {
        private readonly OnlineGallery _gallery;
        public string Name { get; private set; }
        public Bitmap Cover { get; private set; }

        public string GalleryName
        {
            get { return _gallery.Name; }
        }

        public CoverEntry(string name, Bitmap cover, OnlineGallery gallery = null)
        {
            Name = name;
            Cover = cover;
            _gallery = gallery;
        }

        public void AddToGalleryCache(string name)
        {
            if (_gallery != null)
                _gallery.AddCoverToCache(this, name);
        }
    }
}