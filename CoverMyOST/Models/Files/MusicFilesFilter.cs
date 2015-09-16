using System;
using System.Collections.Generic;
using System.Linq;
using CoverMyOST.Models.Files.Base;

namespace CoverMyOST.Models.Files
{
    public class MusicFilesFilter : MusicFilesContainer
    {
        private MusicFileFilter _filter;

        public MusicFilesFilter(IMusicFilesContainer parent)
        {
            Parent = parent;
        }

        public MusicFileFilter Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                Refresh();
            }
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