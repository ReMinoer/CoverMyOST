using System.IO;
using CoverMyOST.Configuration.DataModels;
using CoverMyOST.FileManagers;
using CoverMyOST.Galleries;
using Diese.Serialization;

namespace CoverMyOST.Configuration
{
    public class UserConfiguration
    {
        private const string ConfigFileName = "config.xml";

        private readonly ISerializer<UserConfiguration, UserConfigurationData> _serializer =
            new SerializerXml<UserConfiguration, UserConfigurationData>();

        public MusicFileLoader MusicFileLoader { get; private set; }
        public GalleryManager GalleryManager { get; private set; }

        public bool IsFileExists
        {
            get { return File.Exists(ConfigFileName); }
        }

        public UserConfiguration(MusicFileLoader musicFileLoader, GalleryManager galleryManager)
        {
            MusicFileLoader = musicFileLoader;
            GalleryManager = galleryManager;
        }

        public void Load()
        {
            _serializer.Load(this, ConfigFileName);
        }

        public void Save()
        {
            _serializer.Save(this, ConfigFileName);
        }
    }
}