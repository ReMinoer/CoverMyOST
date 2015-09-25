using System.Linq;
using CoverMyOST.Galleries.Base;
using NLog;

namespace CoverMyOST.Galleries
{
    public class GalleryManager : CoversGalleryContainer<ICoversGallery>
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
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

        public bool TryAddLocalGallery(LocalGallery localGallery, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(localGallery.Name))
            {
                errorMessage = "Gallery name can't be null, empty or composed of white space";
                Logger.Error(errorMessage);
                return false;
            }

            if (GetAllComponentsInChildren().Any(x => x.Name == localGallery.Name))
            {
                errorMessage = string.Format("A gallery with the name \"{0}\" exists already.", localGallery.Name);
                Logger.Error(errorMessage);
                return false;
            }

            Logger.Info("Add local gallery (Name={0}, Path={1})", localGallery.Name, localGallery.Path);
            LocalGalleries.Add(localGallery);
            errorMessage = null;
            return true;
        }

        public void RemoveLocalGallery(string galleryName)
        {
            LocalGallery localGallery = LocalGalleries.First(x => x.Name == galleryName);
            Logger.Info("Remove local gallery (Name={0}, Path={1})", localGallery.Name, localGallery.Path);
            LocalGalleries.Remove(localGallery);
        }
    }
}