using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public interface ICoversGalleryParent : ICoversGallery, IParent<ICoversGallery, ICoversGalleryParent>
    {
    }
}