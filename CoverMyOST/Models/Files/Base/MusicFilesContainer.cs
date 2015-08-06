using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diese.Composition;

namespace CoverMyOST.Models.Files.Base
{
    public abstract class MusicFilesContainer : Component<IMusicFilesContainer, IMusicFilesContainerDecorator>, IMusicFilesContainer
    {
        protected readonly Dictionary<string, MusicFile> _files;
        protected readonly IReadOnlyDictionary<string, MusicFile> _readOnlyFiles;

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return _readOnlyFiles; }
        }

        protected MusicFilesContainer()
        {
            _files = new Dictionary<string, MusicFile>();
            _readOnlyFiles = new ReadOnlyDictionary<string, MusicFile>(_files);
        }

        public abstract void Refresh();
    }
}