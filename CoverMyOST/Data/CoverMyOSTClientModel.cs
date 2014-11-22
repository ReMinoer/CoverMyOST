using System;
using Diese.Modelization;

namespace CoverMyOST.Data
{
    public class CoverMyOSTClientModel : IModel<CoverMyOSTClient>
    {
        public string WorkingDirectory { get; set; }
        public GalleryCollectionModel GalleriesModel { get; set; }

        public CoverMyOSTClientModel() {}

        public CoverMyOSTClientModel(CoverMyOSTClient obj)
        {
            To(obj);
        }

        public void From(CoverMyOSTClient obj)
        {
            WorkingDirectory = obj.WorkingDirectory;
            GalleriesModel = new GalleryCollectionModel(obj.Galleries);
        }

        public void To(CoverMyOSTClient obj)
        {
            try
            {
                obj.ChangeDirectory(WorkingDirectory);
            }
            catch (ArgumentException) {}
            GalleriesModel.To(obj.Galleries);
        }
    }
}