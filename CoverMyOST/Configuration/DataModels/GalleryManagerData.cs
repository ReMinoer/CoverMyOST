using System.Linq;
using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.Configuration.DataModels
{
    public class GalleryManagerData : IConfigurator<GalleryManager>
    {
        public CreatorList<LocalGallery, LocalGalleryData> LocalGalleries { get; set; }
        public MyAnimeListGalleryData MyAnimeList { get; set; }

        public GalleryManagerData()
        {
            LocalGalleries = new CreatorList<LocalGallery, LocalGalleryData>();
            MyAnimeList = new MyAnimeListGalleryData();
        }

        public void From(GalleryManager obj)
        {
            LocalGalleries.From(obj.LocalGalleries.ToList());
            MyAnimeList.From(obj.MyAnimeList);
        }

        public void Configure(GalleryManager obj)
        {
            obj.LocalGalleries.Clear();
            foreach (LocalGalleryData localData in LocalGalleries)
                obj.LocalGalleries.Add(localData.Create());

            MyAnimeList.Configure(obj.MyAnimeList);
        }
    }
}