using System;
using System.Linq;
using CoverMyOST.Galleries;
using CoverMyOST.Galleries.Base;
using CoverMyOST.Windows.Dialogs.Views;

namespace CoverMyOST.Windows.Dialogs
{
    internal class GalleryManagerDialog : IDisposable
    {
        public GalleryManager Model { get; private set; }
        public GalleryManagerView View { get; private set; }

        public GalleryManagerDialog(GalleryManager galleryManager)
        {
            Model = galleryManager;
            View = new GalleryManagerView(Model.GetAllComponentsInChildren<OnlineGallery>(), Model.LocalGalleries);

            View.TryAddLocalGalleryRequest += (sender, args) =>
            {
                string errorMessage;
                if (Model.TryAddLocalGallery(args.Gallery, out errorMessage))
                    View.CompleteAddLocalGallery(args.Gallery);
                else
                    View.FailAddLocalGallery(errorMessage);
            };

            View.RemoveLocalGalleryRequest += (sender, args) =>
            {
                Model.RemoveLocalGallery(args.GalleryName);
            };

            View.ChangeDescriptionRequest += (sender, args) =>
            {
                string description = Model.GetComponentInChildren(args.GalleryName).Description;
                View.CompleteChangeDescriptionRequest(description);
            };

            View.GalleryEnabledChanged += (sender, args) =>
            {
                Model.GetComponentInChildren(args.GalleryName).Enabled = args.Enabled;
            };

            View.UseCacheChanged += (sender, args) =>
            {
                Model.GetAllComponentsInChildren<OnlineGallery>().First(x => x.Name == args.GalleryName).UseCache = args.UseCache;
            };

            View.ClearCacheRequest += (sender, args) =>
            {
                Model.GetAllComponentsInChildren<OnlineGallery>().First(x => x.Name == args.GalleryName).ClearCache();
            };
        }

        public void Dispose()
        {

        }
    }
}