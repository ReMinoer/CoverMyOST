using System.Collections.Generic;
using Diese.Composition;

namespace CoverMyOST.Models.Files.Base
{
    public interface IMusicFilesContainer : IComposite<IMusicFilesContainer, IMusicFilesContainer, IMusicFilesContainer>
    {
        IReadOnlyDictionary<string, MusicFile> Files { get; }
        void Refresh();
    }
}