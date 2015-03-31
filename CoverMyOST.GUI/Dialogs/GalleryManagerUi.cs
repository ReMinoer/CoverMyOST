namespace CoverMyOST.GUI.Dialogs
{
    class GalleryManagerUi
    {
        public IModel Model { get; private set; }
        public IView View { get; private set; }

        public GalleryManagerUi(IModel model, IView view)
        {
            Model = model;
            View = view;
        }

        public interface IModel
        {
        }

        public interface IView
        {
        }
    }
}
