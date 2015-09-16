using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public interface ICoversGalleryContainer<out T> : IContainer<ICoversGallery, ICoversGalleryParent, T>, ICoversGalleryParent
        where T : class, ICoversGallery
    {
    }
}