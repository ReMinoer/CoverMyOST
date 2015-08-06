using Diese.Composition;

namespace CoverMyOST.Models.Files.Base
{
    public interface IMusicFilesContainerDecorator : IMusicFilesContainer, IDecorator<IMusicFilesContainer, IMusicFilesContainerDecorator, IMusicFilesContainer>
    {
    }
}