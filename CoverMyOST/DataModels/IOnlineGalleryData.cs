namespace CoverMyOST.DataModels
{
    public interface IOnlineGalleryModel<TOnlineGallery> : ICoversGalleryModel<TOnlineGallery>
        where TOnlineGallery : OnlineGallery
    {
        bool CacheEnable { get; set; }
    }
}