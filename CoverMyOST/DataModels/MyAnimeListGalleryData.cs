using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class MyAnimeListGalleryData : IConfigurator<MyAnimeListGallery>
    {
        public bool Enable { get; set; }
        public bool UseCache { get; set; }

        public void From(MyAnimeListGallery obj)
        {
            Enable = obj.Enable;
            UseCache = obj.UseCache;
        }

        public void Configure(MyAnimeListGallery obj)
        {
            obj.Enable = Enable;
            obj.UseCache = UseCache;
        }
    }
}