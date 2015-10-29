using CoverMyOST.Galleries;
using Diese.Modelization;

namespace CoverMyOST.Configuration.DataModels.Galleries
{
    public class LocalGalleryData : ICreator<LocalGallery>
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public void From(LocalGallery obj)
        {
            Enabled = obj.Enabled;
            Name = obj.Name;
            Path = obj.Path;
        }

        public LocalGallery Create()
        {
            var obj = new LocalGallery(Name, Path)
            {
                Enabled = Enabled
            };
            return obj;
        }
    }
}