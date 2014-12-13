namespace CoverMyOST.GUI.Dialogs
{
    class GalleryManagerPresenter
    {
        private readonly CoverMyOSTClient _client;
        private readonly IGalleryManagerView _view;

        public GalleryManagerPresenter(IGalleryManagerView view, CoverMyOSTClient client)
        {
            _view = view;
            _client = client;
        }
    }
}
