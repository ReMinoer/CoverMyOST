using CoverMyOST.Models.Galleries;
using CoverMyOST.Windows.Views;

namespace CoverMyOST.Windows.Dialogs
{
    internal class GalleryManagerDialog
    {
        public GalleryManager Model { get; private set; }
        public GalleryManagerView View { get; private set; }

        public GalleryManagerDialog()
        {
            Model = new GalleryManager();
            View = new GalleryManagerView();
        }
    }
}