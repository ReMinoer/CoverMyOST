using System.Collections.Generic;
using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class GalleryCollectionModel : IDataModel<GalleryCollection>
    {
        public List<LocalGalleryModel> LocalDataList { get; set; }
        public MyAnimeListGalleryModel MyAnimeList { get; set; }

        public GalleryCollectionModel()
        {
            LocalDataList = new List<LocalGalleryModel>();
            MyAnimeList = new MyAnimeListGalleryModel();
        }

        public void From(GalleryCollection obj)
        {
            LocalDataList.Clear();
            foreach (LocalGallery local in obj.Local)
            {
                var localData = new LocalGalleryModel();
                localData.From(local);
                LocalDataList.Add(localData);
            }

            MyAnimeList.From(obj.MyAnimeList);
        }

        public void To(GalleryCollection obj)
        {
            obj.ClearLocal();
            foreach (LocalGalleryModel localData in LocalDataList)
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