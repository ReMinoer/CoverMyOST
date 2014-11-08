using System;

namespace CoverMyOST.Data
{
    public class CoverMyOSTClientData : IData<CoverMyOSTClient>
    {
        public string WorkingDirectory { get; set; }
        public GalleryCollectionData GalleriesData { get; set; }

        public CoverMyOSTClientData() {}

        public CoverMyOSTClientData(CoverMyOSTClient obj)
        {
            SetData(obj);
        }

        public void SetData(CoverMyOSTClient obj)
        {
            WorkingDirectory = obj.WorkingDirectory;
            GalleriesData = new GalleryCollectionData(obj.Galleries);
        }

        public void SetObject(CoverMyOSTClient obj)
        {
            try
            {
                obj.ChangeDirectory(WorkingDirectory);
            }
            catch (ArgumentException) {}
            GalleriesData.SetObject(obj.Galleries);
        }
    }
}