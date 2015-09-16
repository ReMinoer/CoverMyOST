using CoverMyOST.Models.Configuration;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class UserConfigurationData : IConfigurator<UserConfiguration>
    {
        public string WorkingDirectory { get; set; }
        public GalleryManagerData GalleriesManager { get; set; }

        public UserConfigurationData()
        {
            GalleriesManager = new GalleryManagerData();
        }

        public void From(UserConfiguration obj)
        {
            WorkingDirectory = obj.MusicFilesLoader.WorkingDirectory;
            GalleriesManager.From(obj.GalleryManager);
        }

        public void Configure(UserConfiguration obj)
        {
            obj.MusicFilesLoader.ChangeDirectory(WorkingDirectory);
            GalleriesManager.Configure(obj.GalleryManager);
        }
    }
}