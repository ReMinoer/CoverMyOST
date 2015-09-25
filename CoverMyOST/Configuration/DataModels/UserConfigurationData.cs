using Diese.Modelization;

namespace CoverMyOST.Configuration.DataModels
{
    public class UserConfigurationData : IConfigurator<UserConfiguration>
    {
        public string WorkingDirectory { get; set; }
        public GalleryManagerData GalleryManager { get; set; }

        public UserConfigurationData()
        {
            GalleryManager = new GalleryManagerData();
        }

        public void From(UserConfiguration obj)
        {
            WorkingDirectory = obj.MusicFileLoader.WorkingDirectory;
            GalleryManager.From(obj.GalleryManager);
        }

        public void Configure(UserConfiguration obj)
        {
            obj.MusicFileLoader.ChangeDirectory(WorkingDirectory);
            GalleryManager.Configure(obj.GalleryManager);
        }
    }
}