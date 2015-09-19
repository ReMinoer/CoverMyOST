using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diese.Composition;

namespace CoverMyOST.FileManagers.Base
{
    public abstract class MusicFileManager : Composite<IMusicFileManager, IMusicFileManager, IMusicFileManager>, IMusicFileManager
    {
        protected readonly Dictionary<string, MusicFile> LocalFiles;
        protected readonly IReadOnlyDictionary<string, MusicFile> ReadOnlyFiles;

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return ReadOnlyFiles; }
        }

        protected MusicFileManager()
        {
            LocalFiles = new Dictionary<string, MusicFile>();
            ReadOnlyFiles = new ReadOnlyDictionary<string, MusicFile>(LocalFiles);
        }

        public void Refresh()
        {
            RefreshLocal();

            foreach (IMusicFileManager container in Components)
                container.Refresh();
        }

        protected abstract void RefreshLocal();
    }
}