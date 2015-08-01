using CoverMyOST.Galleries;

namespace CoverMyOST.DataModels
{
    public class LocalGalleryData : ICoversGalleryModel<LocalGallery>
    {
        public string Name { get; set; }
        public bool Enable { get; set; }

        public void From(LocalGallery obj)
        {
            Name = obj.Name;
            Enable = obj.Enable;
        }

        public void To(LocalGallery obj)
        {
            obj.Enable = Enable;
        }

        public LocalGallery Create()
        {
            var obj = new LocalGallery(Name);
            To(obj);
            return obj;
        }
    }
}