using CoverMyOST.Galleries;
using Diese.Modelization;
using FormPlug.SocketAttributes;

namespace CoverMyOST.Windows.Sockets
{
    public class LocalGalleryCreator : ICreator<LocalGallery>
    {
        [TextSocket]
        public string Name { get; set; }

        [FolderSocket]
        public string Path { get; set; }

        public void From(LocalGallery obj)
        {
            Name = obj.Name;
            Path = obj.Path;
        }

        public LocalGallery Create()
        {
            return new LocalGallery(Name, Path)
            {
                Enabled = true
            };
        }
    }
}