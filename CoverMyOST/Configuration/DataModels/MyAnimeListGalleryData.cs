using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.Configuration.DataModels
{
    public class MyAnimeListGalleryData : IConfigurator<MyAnimeListGallery>
    {
        public bool Enabled { get; set; }
        public bool UseCache { get; set; }

        public void From(MyAnimeListGallery obj)
        {
            Enabled = obj.Enabled;
            UseCache = obj.UseCache;
        }

        public void Configure(MyAnimeListGallery obj)
        {
            obj.Enabled = Enabled;
            obj.UseCache = UseCache;
        }
    }
}