using System.ComponentModel;
using System.Windows.Forms;
using CoverMyOST.Annotations;

namespace CoverMyOST.GUI.Dialogs
{
    public partial class CoverSearchView : Form, ICoverSearchView
    {
        [UsedImplicitly]
        private CoverSearchPresenter _presenter;

        public CoverSearchView(CoverMyOSTClient client)
        {
            InitializeComponent();

            _presenter = new CoverSearchPresenter(this, client);
        }

        public ListView ListView { get { return listView; } }

        public Label CountLabel { get { return countLabel; } }
        public TextBox FileTextBox { get { return fileTextBox; } }
        public TextBox AlbumTextBox { get { return albumTextBox; } }

        public PictureBox CoverPreview { get { return coverPreview; } }

        public Button PlayButton { get { return playButton; } }
        public Button ApplyButton { get { return applyButton; } }

        public BackgroundWorker BackgroundWorker { get { return backgroundWorker; } }

        public ToolStripProgressBar SearchProgressBar { get { return searchProgressBar; } }
        public ToolStripStatusLabel StatusLabel { get { return statusLabel; } }

        public void CloseDialog()
        {
            Close();
        }
    }

    public interface ICoverSearchView
    {
        Label CountLabel { get; }
        TextBox FileTextBox { get; }
        TextBox AlbumTextBox { get; }

        ListView ListView { get; }

        PictureBox CoverPreview { get; }

        Button PlayButton { get; }
        Button ApplyButton { get; }

        BackgroundWorker BackgroundWorker { get; }

        ToolStripProgressBar SearchProgressBar { get; }
        ToolStripStatusLabel StatusLabel { get; }

        void CloseDialog();
    }
}