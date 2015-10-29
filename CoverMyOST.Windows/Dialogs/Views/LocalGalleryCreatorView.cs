using System.Windows.Forms;
using CoverMyOST.Windows.Sockets;
using FormPlug;
using FormPlug.WindowsForm.Plugs;

namespace CoverMyOST.Windows.Dialogs.Views
{
    public partial class LocalGalleryCreatorView : Form
    {
        public LocalGalleryCreatorView(LocalGalleryCreator localGalleryCreator)
        {
            InitializeComponent();

            var plugableView = new PlugablePanel(this);
            plugableView.Connect(localGalleryCreator);
        }

        private sealed class PlugablePanel : PlugablePanel<LocalGalleryCreatorView, LocalGalleryCreator, Control>
        {
            public PlugablePanel(LocalGalleryCreatorView panel)
                : base(panel)
            {
            }

            protected override void CreatePlugs(LocalGalleryCreatorView panel)
            {
                AddPlug<TextPlug>(Panel.nameTextBox, "Name");
                AddPlug<FolderPlug>(Panel.pathDialogButton, "Path");
            }
        }
    }
}