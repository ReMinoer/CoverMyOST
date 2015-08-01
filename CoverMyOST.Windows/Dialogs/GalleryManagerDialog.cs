using CoverMyOST.Models.Managers;
using CoverMyOST.Windows.Views;

namespace CoverMyOST.Windows.Dialogs
{
    internal class GalleryManagerDialog
    {
        public GalleryManagerModel Model { get; private set; }
        public GalleryManagerView View { get; private set; }

        public GalleryManagerDialog()
        {
            Model = new GalleryManagerModel();
            View = new GalleryManagerView();
        }
    }
}