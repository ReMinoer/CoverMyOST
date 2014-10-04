namespace CoverMyOST.Data
{
    public class OnlineGalleryData : CoversGalleryData<OnlineGallery>
    {
        public override string Name { get; set; }
        public bool CacheEnable { get; set; }

        public OnlineGalleryData(OnlineGallery obj)
        {
            SetData(obj);
        }

        public sealed override void SetData(OnlineGallery obj)
        {
            base.SetData(obj);
            Name = obj.Name;
            CacheEnable = obj.CacheEnable;
        }

        public sealed override void SetObject(OnlineGallery obj)
        {
            base.SetObject(obj);
            obj.CacheEnable = CacheEnable;
        }
    }
}