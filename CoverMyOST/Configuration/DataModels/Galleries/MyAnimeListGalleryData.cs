using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.Configuration.DataModels.Galleries
{
    public class MyAnimeListGalleryData : IConfigurator<MyAnimeListGallery>
    {
        public bool Enabled { get; set; }
        public bool UseCache { get; set; }
        public string Username { get; set; }
        public byte[] CryptedPassword { get; set; }

        public void From(MyAnimeListGallery obj)
        {
            Enabled = obj.Enabled;
            UseCache = obj.UseCache;
            Username = obj.Username;
            CryptedPassword = obj.CryptedPassword;
        }

        public void Configure(MyAnimeListGallery obj)
        {
            obj.Enabled = Enabled;
            obj.UseCache = UseCache;
            obj.Username = Username;
            obj.CryptedPassword = CryptedPassword;
        }
    }
}