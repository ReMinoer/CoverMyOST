namespace CoverMyOST.GUI.Dialogs
{
    internal class CoverSearchPresenter
    {
        private CoverMyOSTClient _client;
        private ICoverSearchView _view;

        public CoverSearchPresenter(ICoverSearchView view, CoverMyOSTClient client)
        {
            _view = view;

            _client = client;
        }
    }
}