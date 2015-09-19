using CoverMyOST.Galleries.Base;

namespace CoverMyOST.Galleries
{
    public class GalleryManager : CoversGalleryContainer<ICoversGallery>
    {
        public ICoversGalleryComposite<LocalGallery> LocalGalleries { get; private set; }
        public MyAnimeListGallery MyAnimeList { get; private set; }

        public GalleryManager()
            : base(2)
        {
            Components[0] = LocalGalleries = new CoversGalleryComposite<LocalGallery>();
            Components[1] = MyAnimeList = new MyAnimeListGallery();
        }
    }
}