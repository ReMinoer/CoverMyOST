using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public interface ICoversGalleryComposite<T> : IComposite<ICoversGallery, ICoversGalleryParent, T>, ICoversGalleryParent
        where T : class, ICoversGallery
    {
    }
}