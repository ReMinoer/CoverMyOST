using System;
using System.IO;
using CoverMyOST.DataModels;
using CoverMyOST.Models.Files;
using CoverMyOST.Models.Galleries;
using Diese.Serialization;

namespace CoverMyOST.Models.Configuration
{
    public class UserConfiguration
    {
        static private readonly ISerializer<UserConfiguration, UserConfigurationData> Serializer =
            new SerializerXml<UserConfiguration, UserConfigurationData>();

        private const string ConfigFileName = "CoverMyOST-config.xml";

        public MusicFilesLoader MusicFilesLoader { get; private set; }
        public GalleryManager GalleryManager { get; private set; }

        public bool IsFileExists
        {
            get { return File.Exists(ConfigFileName); }
        }

        public UserConfiguration(MusicFilesLoader musicFilesLoader, GalleryManager galleryManager)
        {
            MusicFilesLoader = musicFilesLoader;
            GalleryManager = galleryManager;
        }

        public void Load()
        {
            Serializer.Load(this, ConfigFileName);
        }

        public void Save()
        {
            Serializer.Save(this, ConfigFileName);
        }
    }
}