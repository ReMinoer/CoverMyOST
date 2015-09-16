using System.Linq;
using CoverMyOST.Galleries;
using CoverMyOST.Models.Galleries;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class GalleryManagerData : IConfigurator<GalleryManager>
    {
        public CreatorList<LocalGallery, LocalGalleryData> LocalData { get; set; }
        public MyAnimeListGalleryData MyAnimeList { get; set; }

        public GalleryManagerData()
        {
            LocalData = new CreatorList<LocalGallery, LocalGalleryData>();
            MyAnimeList = new MyAnimeListGalleryData();
        }

        public void From(GalleryManager obj)
        {
            LocalData.From(obj.LocalGalleries.ToList());
            MyAnimeList.From(obj.MyAnimeList);
        }

        public void Configure(GalleryManager obj)
        {
            obj.LocalGalleries.Clear();
            foreach (LocalGalleryData localData in LocalData)
                obj.LocalGalleries.Add(localData.Create());

            MyAnimeList.Configure(obj.MyAnimeList);
        }
    }
}