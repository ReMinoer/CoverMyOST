using System;
using System.Collections.Generic;
using System.Linq;
using CoverMyOST.Models.Files.Base;

namespace CoverMyOST.Models.Files
{
    public class MusicFilesFilter : MusicFilesContainerDecorator
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

        protected override void Refresh(IReadOnlyDictionary<string, MusicFile> componentFiles)
        {
            IEnumerable<KeyValuePair<string, MusicFile>> filteredFiles;
            switch (_filter)
            {
                case MusicFileFilter.None:
                    filteredFiles = componentFiles;
                    break;
                case MusicFileFilter.AlbumSpecified:
                    filteredFiles = componentFiles.Where(e => !string.IsNullOrEmpty(e.Value.Album));
                    break;
                case MusicFileFilter.NoAlbum:
                    filteredFiles = componentFiles.Where(e => string.IsNullOrEmpty(e.Value.Album));
                    break;
                case MusicFileFilter.CoverSpecified:
                    filteredFiles = componentFiles.Where(e => e.Value.Cover != null);
                    break;
                case MusicFileFilter.NoCover:
                    filteredFiles = componentFiles.Where(e => e.Value.Cover == null);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            _files.Clear();

            foreach (KeyValuePair<string, MusicFile> pair in filteredFiles)
                _files.Add(pair.Key, pair.Value);
        }
    }
}