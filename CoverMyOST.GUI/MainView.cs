using System.ComponentModel;
using System.Windows.Forms;
using CoverMyOST.Annotations;

namespace CoverMyOST.GUI
{
    public partial class MainView : Form, IMainView
    {
        [UsedImplicitly]
        private MainPresenter _presenter;

        public MainView()
        {
            InitializeComponent();

            _presenter = new MainPresenter(this);
        }

        public ToolStripButton OpenButton { get { return openButton; } }
        public ToolStripButton SaveAllButton { get { return saveAllButton; } }
        public ToolStripComboBox FilterComboBox { get { return filterComboBox; } }
        public ToolStripButton GalleryManagerButton { get { return galleryManagerButton; } }
        public ToolStripButton CoversButton { get { return coversButton; } }

        public DataGridView GridView { get { return gridView; } }

        public string StatusStripLabel { get { return statusStripLabel.Text; } set { statusStripLabel.Text = value; } }

        public event CancelEventHandler ClosingProgram { add { Closing += value; } remove { Closing -= value; } }
    }

    public interface IMainView
    {
        ToolStripButton OpenButton { get; }
        ToolStripButton SaveAllButton { get; }
        ToolStripComboBox FilterComboBox { get; }
        ToolStripButton GalleryManagerButton { get; }
        ToolStripButton CoversButton { get; }

        DataGridView GridView { get; }

        string StatusStripLabel { get; set; }

        event CancelEventHandler ClosingProgram;
    }
}