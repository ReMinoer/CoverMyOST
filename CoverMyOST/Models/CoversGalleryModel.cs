using Diese.Modelization;

namespace CoverMyOST.Data
{
    public abstract class CoversGalleryModel<TCoversGallery> : IModel<TCoversGallery>
        where TCoversGallery : ICoversGallery
    {
        public abstract string Name { get; set; }
        public bool Enable { get; set; }

        public virtual void From(TCoversGallery obj)
        {
            Enable = obj.Enable;
        }

        public virtual void To(TCoversGallery obj)
        {
            obj.Enable = Enable;
        }
    }
}