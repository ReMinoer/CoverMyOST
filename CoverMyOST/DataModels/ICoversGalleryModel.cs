using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public interface ICoversGalleryModel<TCoversGallery> : IDataModel<TCoversGallery>
        where TCoversGallery : ICoversGallery
    {
        bool Enable { get; set; }
    }
}