﻿using CoverMyOST.Ui;
using CoverMyOST.Windows.Views;

namespace CoverMyOST.Windows.Dialogs
{
    class GalleryManagerDialog
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
