using CoverMyOST.Galleries;

namespace CoverMyOST.DataModels
{
    public class MyAnimeListGalleryData : IOnlineGalleryModel<MyAnimeListGallery>
    {
        public bool Enable { get; set; }
        public bool CacheEnable { get; set; }

        public void From(MyAnimeListGallery obj)
        {
            Enable = obj.Enable;
            CacheEnable = obj.CacheEnable;
        }

        public void To(MyAnimeListGallery obj)
        {
            obj.Enable = Enable;
            obj.CacheEnable = CacheEnable;
        }

        public MyAnimeListGallery Create()
        {
            var obj = new MyAnimeListGallery();
            To(obj);
            return obj;
        }
    }
}