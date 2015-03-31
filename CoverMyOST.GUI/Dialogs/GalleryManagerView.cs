using System.Windows.Forms;
using Ui = CoverMyOST.GUI.Dialogs.GalleryManagerUi;

namespace CoverMyOST.GUI.Dialogs
{
    public partial class GalleryManagerView : Form, Ui.IView
    {
        public GalleryManagerView()
        {
            InitializeComponent();
        }
    }
}