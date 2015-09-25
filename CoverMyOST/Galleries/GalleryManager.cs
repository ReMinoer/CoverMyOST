using System.Linq;
using CoverMyOST.Galleries.Base;

namespace CoverMyOST.Galleries
{
    public class GalleryManager : CoversGalleryContainer<ICoversGallery>
    {
        public ICoversGalleryComposite<LocalGallery> LocalGalleries { get; private set; }
        public MyAnimeListGallery MyAnimeList { get; private set; }

        public override string Description
        {
            get { return "Galllery manager"; }
        }

        public GalleryManager()
            : base(2)
        {
            Components[0] = LocalGalleries = new CoversGalleryComposite<LocalGallery>();
            Components[1] = MyAnimeList = new MyAnimeListGallery();
        }

        public bool TryAddLocalGallery(LocalGallery localGallery)
        {
            if (string.IsNullOrWhiteSpace(localGallery.Name)
                || GetAllComponentsInChildren().Any(x => x.Name == localGallery.Name))
                return false;

            LocalGalleries.Add(localGallery);
            return true;
        }

        public void RemoveLocalGallery(string galleryName)
        {
            LocalGallery localGallery = LocalGalleries.First(x => x.Name == galleryName);
            LocalGalleries.Remove(localGallery);
        }
    }
}