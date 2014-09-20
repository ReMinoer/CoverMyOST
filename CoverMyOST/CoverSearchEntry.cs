using System.Drawing;

namespace CoverMyOST
{
    public class CoverSearchEntry
    {
        public string Name { get; private set; }
        public Bitmap Cover { get; private set; }
        private readonly AbstractOnlineGallery _gallery;

        public CoverSearchEntry(string name, Bitmap cover, AbstractOnlineGallery gallery = null)
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