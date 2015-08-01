using System;
using Diese.Modelization;

namespace CoverMyOST.DataModels
{
    public class CoverMyOSTClientData : IDataModel<CoverMyOSTClient>
    {
        public string WorkingDirectory { get; set; }
        public GalleryCollectionData GalleriesCollection { get; set; }

        public CoverMyOSTClientData()
        {
            GalleriesCollection = new GalleryCollectionData();
        }

        public void From(CoverMyOSTClient obj)
        {
            WorkingDirectory = obj.WorkingDirectory;
            GalleriesCollection.From(obj.Galleries);
        }

        public void To(CoverMyOSTClient obj)
        {
            try
            {
                obj.ChangeDirectory(WorkingDirectory);
            }
            catch (ArgumentException)
            {
            }
            GalleriesCollection.To(obj.Galleries);
        }

        public CoverMyOSTClient Create()
        {
            var obj = new CoverMyOSTClient();
            To(obj);
            return obj;
        }
    }
}