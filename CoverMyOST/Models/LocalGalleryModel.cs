using CoverMyOST.Annotations;
using CoverMyOST.Galleries;

namespace CoverMyOST.Data
{
    public class LocalGalleryModel : CoversGalleryModel<LocalGallery>
    {
        public override string Name { get; set; }

        [UsedImplicitly]
        public LocalGalleryModel() {}

        public LocalGalleryModel(LocalGallery obj)
        {
            From(obj);
        }

        public sealed override void From(LocalGallery obj)
        {
            base.From(obj);
            Name = obj.Name;
        }

        public sealed override void To(LocalGallery obj)
        {
            base.To(obj);
            obj.Name = Name;
        }
    }
}