using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diese.Composition;

namespace CoverMyOST.Models.Files.Base
{
    public abstract class MusicFilesContainerDecorator : Decorator<IMusicFilesContainer, IMusicFilesContainerDecorator, IMusicFilesContainer>, IMusicFilesContainerDecorator
    {
        protected readonly Dictionary<string, MusicFile> _files;
        protected readonly IReadOnlyDictionary<string, MusicFile> _readOnlyFiles;

        public IReadOnlyDictionary<string, MusicFile> Files
        {
            get { return _readOnlyFiles; }
        }

        protected MusicFilesContainerDecorator()
        {
            _files = new Dictionary<string, MusicFile>();
            _readOnlyFiles = new ReadOnlyDictionary<string, MusicFile>(_files);
        }

        public void Refresh()
        {
            Refresh(Component.Files);
            Parent.Refresh();
        }

        protected abstract void Refresh(IReadOnlyDictionary<string, MusicFile> componentFiles);
    }
}