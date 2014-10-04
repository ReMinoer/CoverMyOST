using CoverMyOST.Galleries;

namespace CoverMyOST.Data
{
    public class LocalGalleryData : CoversGalleryData<LocalGallery>
    {
        public override string Name { get; set; }

        public LocalGalleryData(LocalGallery obj)
        {
            SetData(obj);
        }

        public sealed override void SetData(LocalGallery obj)
        {
            base.SetData(obj);
            Name = obj.Name;
        }

        public sealed override void SetObject(LocalGallery obj)
        {
            base.SetObject(obj);
            obj.Name = Name;
        }
    }
}