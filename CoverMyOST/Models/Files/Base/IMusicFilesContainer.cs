using System.Collections.Generic;
using Diese.Composition;

namespace CoverMyOST.Models.Files.Base
{
    public interface IMusicFilesContainer : IComponent<IMusicFilesContainer, IMusicFilesContainerDecorator>
    {
        IReadOnlyDictionary<string, MusicFile> Files { get; }
        void Refresh();
    }
}