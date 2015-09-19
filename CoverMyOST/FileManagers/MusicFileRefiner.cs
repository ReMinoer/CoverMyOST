using System;
using System.Collections.Generic;
using System.Linq;
using CoverMyOST.FileManagers.Base;

namespace CoverMyOST.FileManagers
{
    public class MusicFileRefiner : MusicFileManager
    {
        private MusicFileFilter _filter;

        public MusicFileFilter Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                Refresh();
            }
        }

        public MusicFileRefiner(IMusicFileManager parent)
        {
            Parent = parent;
        }

        protected override void RefreshLocal()
        {
            IEnumerable<KeyValuePair<string, MusicFile>> filteredFiles;
            switch (_filter)
            {
                case MusicFileFilter.None:
                    filteredFiles = Parent.Files;
                    break;
                case MusicFileFilter.AlbumSpecified:
                    filteredFiles = Parent.Files.Where(e => !string.IsNullOrEmpty(e.Value.Album));
                    break;
                case MusicFileFilter.NoAlbum:
                    filteredFiles = Parent.Files.Where(e => string.IsNullOrEmpty(e.Value.Album));
                    break;
                case MusicFileFilter.CoverSpecified:
                    filteredFiles = Parent.Files.Where(e => e.Value.Cover != null);
                    break;
                case MusicFileFilter.NoCover:
                    filteredFiles = Parent.Files.Where(e => e.Value.Cover == null);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            LocalFiles.Clear();

            foreach (KeyValuePair<string, MusicFile> pair in filteredFiles)
                LocalFiles.Add(pair.Key, pair.Value);
        }
    }
}