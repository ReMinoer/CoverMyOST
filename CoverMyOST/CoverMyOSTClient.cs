using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        static private readonly Exporter<CoverMyOSTClient, CoverMyOSTClientData> Exporter =
            new Exporter<CoverMyOSTClient, CoverMyOSTClientData>();

        public CoverMyOSTClient()
        {
            WorkingDirectory = "";
            Galleries = new GalleryCollection();

            _allFiles = new Dictionary<string, MusicFile>();
            FilterFiles(MusicFileFilter.None);
        }

        public void LoadConfiguration()
        {
            Exporter.LoadXml(this, ConfigFileName);
        }

        public void SaveConfiguration()
        {
            Exporter.SaveXml(this, ConfigFileName);
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
    }
}