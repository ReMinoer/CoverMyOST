using System.Collections.Generic;
using Diese.Composition;

namespace CoverMyOST.FileManagers.Base
{
    public interface IMusicFileManager : IComposite<IMusicFileManager, IMusicFileManager, IMusicFileManager>
    {
        IReadOnlyDictionary<string, MusicFile> Files { get; }
        void Refresh();
    }
}