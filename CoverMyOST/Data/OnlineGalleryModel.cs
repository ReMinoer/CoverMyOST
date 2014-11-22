using CoverMyOST.Annotations;

namespace CoverMyOST.Data
{
    public class OnlineGalleryModel : CoversGalleryModel<OnlineGallery>
    {
        public override string Name { get; set; }
        public bool CacheEnable { get; set; }

        [UsedImplicitly]
        public OnlineGalleryModel() {}

        public OnlineGalleryModel(OnlineGallery obj)
        {
            From(obj);
        }

        public sealed override void From(OnlineGallery obj)
        {
            base.From(obj);
            Name = obj.Name;
            CacheEnable = obj.CacheEnable;
        }

        public sealed override void To(OnlineGallery obj)
        {
            base.To(obj);
            obj.CacheEnable = CacheEnable;
        }
    }
}