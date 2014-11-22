using System.Collections.Generic;
using System.Linq;
using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.Data
{
    public class GalleryCollectionModel : IModel<GalleryCollection>
    {
        public List<LocalGalleryModel> LocalDataList { get; set; }
        public List<OnlineGalleryModel> OnlineData { get; set; }

        public GalleryCollectionModel()
        {
            LocalDataList = new List<LocalGalleryModel>();
            OnlineData = new List<OnlineGalleryModel>();
        }

        public GalleryCollectionModel(GalleryCollection obj)
            : this()
        {
            From(obj);
        }

        public void From(GalleryCollection obj)
        {
            LocalDataList.Clear();
            foreach (LocalGallery local in obj.Local)
                LocalDataList.Add(new LocalGalleryModel(local));

            OnlineData.Clear();
            foreach (ICoversGallery online in obj.List.Where(gallery => gallery is OnlineGallery))
                OnlineData.Add(new OnlineGalleryModel((online as OnlineGallery)));
        }

        public void To(GalleryCollection obj)
        {
            obj.ClearLocal();
            foreach (LocalGalleryModel localData in LocalDataList)
            {
                var local = new LocalGallery(localData.Name);
                localData.To(local);
                obj.AddLocal(local);
            }

            foreach (OnlineGalleryModel onlineData in OnlineData)
                onlineData.To(obj[onlineData.Name] as OnlineGallery);
        }
    }
}