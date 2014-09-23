﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CoverMyOST.Data;

namespace CoverMyOST
{
	public class CoverMyOSTClient
    {
        public string WorkingDirectory { get; private set; }
        public GalleryCollection Galleries { get; private set; }

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return new ReadOnlyDictionary<string, MusicFile>(_filteredFiles); }
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

        private readonly Dictionary<string, MusicFile> _allFiles;
        private MusicFileFilter _filter;
        private Dictionary<string, MusicFile> _filteredFiles;

		private const string ConfigFileName = "CoverMyOST.config";

		private static Exporter<CoverMyOSTClient, CoverMyOSTClientData> _exporter
		= new Exporter<CoverMyOSTClient, CoverMyOSTClientData>();

        public CoverMyOSTClient()
        {
            WorkingDirectory = "";
            Galleries = new GalleryCollection();

            _allFiles = new Dictionary<string, MusicFile>();
            FilterFiles(MusicFileFilter.None);
        }

        public CoverMyOSTClient(string workingDirectory)
            : this()
        {
            ChangeDirectory(workingDirectory);
        }

		public void LoadConfiguration()
		{
			_exporter.LoadXML(this, ConfigFileName);
		}

		public void SaveConfiguration()
		{
			_exporter.SaveXML(this, ConfigFileName);
		}

        public void ChangeDirectory(string path)
        {
            _allFiles.Clear();

            foreach (string file in Directory.EnumerateFiles(path))
            {
                string fullpath = Path.GetFullPath(file);
                _allFiles.Add(fullpath, new MusicFile(fullpath));
            }

            FilterFiles(MusicFileFilter.None);
            WorkingDirectory = path;
        }

        private void FilterFiles(MusicFileFilter filter)
        {
            switch (filter)
            {
                case MusicFileFilter.None:
                    _filteredFiles = _allFiles;
                    break;
                case MusicFileFilter.NoAlbum:
                    _filteredFiles = _allFiles.Where(e => string.IsNullOrEmpty(e.Value.Album)).
                                               ToDictionary(e => e.Key, e => e.Value);
                    break;
                case MusicFileFilter.NoCover:
                    _filteredFiles = _allFiles.Where(e => e.Value.Cover == null).ToDictionary(e => e.Key, e => e.Value);
                    break;
            }
        }

        public void SaveAll()
        {
            foreach (MusicFile musicFile in _allFiles.Values)
                musicFile.Save();
        }

        public CoverSearchResult SearchCover(string filePath)
        {
            return Galleries.SearchCover(_allFiles[filePath].Album);
        }

        public CoverSearchResult SearchCover<TCoversGallery>(string filePath) where TCoversGallery : ICoversGallery
        {
            return Galleries.SearchCover<TCoversGallery>(_allFiles[filePath].Album);
        }

        public CoverSearchResult SearchCoverOnline<TOnlineGallery>(string filePath)
            where TOnlineGallery : OnlineGallery
        {
            return Galleries.SearchCoverOnline<TOnlineGallery>(_allFiles[filePath].Album);
        }

        public CoverEntry SearchCoverCached<TOnlineGallery>(string filePath)
            where TOnlineGallery : OnlineGallery
        {
            return Galleries.SearchCoverCached<TOnlineGallery>(_allFiles[filePath].Album);
        }

        public CoverSearchResult SearchCover(MusicFile musicFile)
        {
            return SearchCover(musicFile.Path);
        }

        public CoverSearchResult SearchCover<TCoversGallery>(MusicFile musicFile) where TCoversGallery : ICoversGallery
        {
            return SearchCover<TCoversGallery>(musicFile.Path);
        }

        public CoverSearchResult SearchCoverOnline<TOnlineGallery>(MusicFile musicFile)
            where TOnlineGallery : OnlineGallery
        {
            return SearchCoverOnline<TOnlineGallery>(musicFile.Path);
        }

        public CoverEntry SearchCoverCached<TOnlineGallery>(MusicFile musicFile)
            where TOnlineGallery : OnlineGallery
        {
            return Galleries.SearchCoverCached<TOnlineGallery>(musicFile.Path);
        }
    }
}