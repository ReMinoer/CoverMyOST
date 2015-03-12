using System.Windows.Forms;
using CoverMyOST.Annotations;

namespace CoverMyOST.GUI.Dialogs
{
    public partial class GalleryManagerView : Form, IGalleryManagerView
    {
        [UsedImplicitly] private GalleryManagerPresenter _presenter;

        public GalleryManagerView(CoverMyOSTClient client)
        {
            InitializeComponent();

            _presenter = new GalleryManagerPresenter(this, client);
        }
    }

    public interface IGalleryManagerView
    {
    }
}