using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class LocalGalleryData : ICreator<LocalGallery>
    {
        public bool Enable { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public void From(LocalGallery obj)
        {
            Enable = obj.Enable;
            Name = obj.Name;
            Path = obj.Path;
        }

        public LocalGallery Create()
        {
            var obj = new LocalGallery(Name, Path)
            {
                Enable = Enable
            };
            return obj;
        }
    }
}