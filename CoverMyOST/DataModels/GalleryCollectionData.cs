using System.Collections.Generic;
using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class GalleryCollectionData : IDataModel<GalleryCollection>
    {
        public List<LocalGalleryData> LocalDataList { get; set; }
        public MyAnimeListGalleryData MyAnimeList { get; set; }

        public GalleryCollectionData()
        {
            LocalDataList = new List<LocalGalleryData>();
            MyAnimeList = new MyAnimeListGalleryData();
        }

        public void From(GalleryCollection obj)
        {
            LocalDataList.Clear();
            foreach (LocalGallery local in obj.Local)
            {
                var localData = new LocalGalleryData();
                localData.From(local);
                LocalDataList.Add(localData);
            }

            MyAnimeList.From(obj.MyAnimeList);
        }

        public void To(GalleryCollection obj)
        {
            obj.ClearLocal();
            foreach (LocalGalleryData localData in LocalDataList)
                obj.AddLocal(localData.Create());

            MyAnimeList.To(obj.MyAnimeList);
        }

        public GalleryCollection Create()
        {
            var obj = new GalleryCollection();
            To(obj);
            return obj;
        }
    }
}