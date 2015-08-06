﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoverMyOST.DataModels;
using Diese.Serialization;
using TagLib;
using File = System.IO.File;

#if !MONO

#else
using System.Diagnostics;
#endif

namespace CoverMyOST
{
    public class CoverMyOSTClient
    {
        static private readonly ISerializer<CoverMyOSTClient, CoverMyOSTClientData> Serializer =
            new SerializerXml<CoverMyOSTClient, CoverMyOSTClientData>();

        private const string ConfigFileName = "CoverMyOST-config.xml";
        private readonly Dictionary<string, MusicFileEntry> _allFiles;
        private MusicFileFilter _filter;
        private Dictionary<string, MusicFileEntry> _filteredFiles;
        public string WorkingDirectory { get; private set; }
        public GalleryCollection Galleries { get; private set; }

        public IReadOnlyDictionary<string, MusicFileEntry> AllFiles
        {
            get { return new ReadOnlyDictionary<string, MusicFileEntry>(_allFiles); }
        }

        public IReadOnlyDictionary<string, MusicFileEntry> AllSelectedFiles
        {
            get
            {
                return
                    new ReadOnlyDictionary<string, MusicFileEntry>(
                        _allFiles.Where(x => x.Value.Selected).ToDictionary(x => x.Key, x => x.Value));
            }
        }

        public IReadOnlyDictionary<string, MusicFileEntry> Files
        {
            get { return new ReadOnlyDictionary<string, MusicFileEntry>(_filteredFiles); }
        }

        public MusicFileFilter Filter
        {
            get { return _filter; }
            set
            {
                FilterFiles(value);
                _filter = value;
            }
        }

        public CoverMyOSTClient()
        {
            WorkingDirectory = "";
            Galleries = new GalleryCollection();

            _allFiles = new Dictionary<string, MusicFileEntry>();
            FilterFiles(MusicFileFilter.None);
        }

        public void LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFileName))
                    Serializer.Load(this, ConfigFileName);
            }
            catch (Exception)
            {
                MessageBox.Show(@"Config file is corrupted. Fix it by yourself or delete it to reset.", "CoverMyOST",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public void SaveConfiguration()
        {
            Serializer.Save(this, ConfigFileName);
        }

        public void ChangeDirectory(string path)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException("Directory specify does not exist.");

            _allFiles.Clear();

            foreach (string file in Directory.EnumerateFiles(path))
            {
                string fullpath = Path.GetFullPath(file);
                try
                {
                    var musicFile = new MusicFileEntry(fullpath);
                    _allFiles.Add(fullpath, musicFile);
                }
                catch (UnsupportedFormatException)
                {
                }
            }

            FilterFiles(MusicFileFilter.None);
            WorkingDirectory = path;
        }

        public void SaveAll()
        {
            foreach (MusicFileEntry musicFile in _allFiles.Values)
                try
                {
                    musicFile.Save();
                }
                catch (IOException)
                {
                    musicFile.Save();
                }
        }

        public CoverSearchResult SearchCover(string filePath)
        {
            return SearchCoverAsync(filePath).Result;
        }

        public CoverSearchResult SearchCover(MusicFile musicFile)
        {
            return SearchCoverAsync(musicFile).Result;
        }

        public async Task<CoverSearchResult> SearchCoverAsync(string filePath)
        {
            return await Galleries.SearchCoverAsync(_allFiles[filePath].Album);
        }

        public async Task<CoverSearchResult> SearchCoverAsync(MusicFile musicFile)
        {
            return await SearchCoverAsync(musicFile.Path);
        }

        public CoverSearchResult SearchCover<TCoversGallery>(string filePath) where TCoversGallery : ICoversGallery
        {
            return SearchCoverAsync<TCoversGallery>(filePath).Result;
        }

        public CoverSearchResult SearchCover<TCoversGallery>(MusicFile musicFile) where TCoversGallery : ICoversGallery
        {
            return SearchCoverAsync<TCoversGallery>(musicFile).Result;
        }

        public async Task<CoverSearchResult> SearchCoverAsync<TCoversGallery>(string filePath)
            where TCoversGallery : ICoversGallery
        {
            return await Galleries.SearchCoverAsync<TCoversGallery>(_allFiles[filePath].Album);
        }

        public async Task<CoverSearchResult> SearchCoverAsync<TCoversGallery>(MusicFile musicFile)
            where TCoversGallery : ICoversGallery
        {
            return await SearchCoverAsync<TCoversGallery>(musicFile.Path);
        }

        public CoverSearchResult SearchCoverOnline<TOnlineGallery>(string filePath) where TOnlineGallery : OnlineGallery
        {
            return SearchCoverOnlineAsync<TOnlineGallery>(filePath).Result;
        }

        public CoverSearchResult SearchCoverOnline<TOnlineGallery>(MusicFile musicFile)
            where TOnlineGallery : OnlineGallery
        {
            return SearchCoverOnlineAsync<TOnlineGallery>(musicFile).Result;
        }

        public async Task<CoverSearchResult> SearchCoverOnlineAsync<TOnlineGallery>(string filePath)
            where TOnlineGallery : OnlineGallery
        {
            return await Galleries.SearchCoverOnlineAsync<TOnlineGallery>(_allFiles[filePath].Album);
        }

        public async Task<CoverSearchResult> SearchCoverOnlineAsync<TOnlineGallery>(MusicFile musicFile)
            where TOnlineGallery : OnlineGallery
        {
            return await SearchCoverOnlineAsync<TOnlineGallery>(musicFile.Path);
        }

        public CoverEntry SearchCoverCached<TOnlineGallery>(string filePath) where TOnlineGallery : OnlineGallery
        {
            return Galleries.SearchCoverCached<TOnlineGallery>(_allFiles[filePath].Album);
        }

        public CoverEntry SearchCoverCached<TOnlineGallery>(MusicFile musicFile) where TOnlineGallery : OnlineGallery
        {
            return Galleries.SearchCoverCached<TOnlineGallery>(musicFile.Path);
        }

        private void FilterFiles(MusicFileFilter filter)
        {
            switch (filter)
            {
                case MusicFileFilter.None:
                    _filteredFiles = _allFiles;
                    break;
                case MusicFileFilter.AlbumSpecified:
                    _filteredFiles = _allFiles.Where(e => !string.IsNullOrEmpty(e.Value.Album)).
                        ToDictionary(e => e.Key, e => e.Value);
                    break;
                case MusicFileFilter.NoAlbum:
                    _filteredFiles = _allFiles.Where(e => string.IsNullOrEmpty(e.Value.Album)).
                        ToDictionary(e => e.Key, e => e.Value);
                    break;
                case MusicFileFilter.CoverSpecified:
                    _filteredFiles = _allFiles.Where(e => e.Value.Cover != null).ToDictionary(e => e.Key, e => e.Value);
                    break;
                case MusicFileFilter.NoCover:
                    _filteredFiles = _allFiles.Where(e => e.Value.Cover == null).ToDictionary(e => e.Key, e => e.Value);
                    break;
            }
        }
    }
}