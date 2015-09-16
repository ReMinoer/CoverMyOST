using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diese.Composition;

namespace CoverMyOST.Models.Files.Base
{
    public abstract class MusicFilesContainer : Composite<IMusicFilesContainer, IMusicFilesContainer, IMusicFilesContainer>, IMusicFilesContainer
    {
        protected readonly Dictionary<string, MusicFile> LocalFiles;
        protected readonly IReadOnlyDictionary<string, MusicFile> ReadOnlyFiles;

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return ReadOnlyFiles; }
        }

        protected MusicFilesContainer()
        {
            LocalFiles = new Dictionary<string, MusicFile>();
            ReadOnlyFiles = new ReadOnlyDictionary<string, MusicFile>(LocalFiles);
        }

        public void Refresh()
        {
            RefreshLocal();

            foreach (IMusicFilesContainer container in Components)
                container.Refresh();
        }

        protected abstract void RefreshLocal();
    }
}