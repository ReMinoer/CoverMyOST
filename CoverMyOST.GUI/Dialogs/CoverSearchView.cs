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

        public Label FileLabel { get { return fileLabel; } }
        public Label AlbumLabel { get { return albumLabel; } }

        public PictureBox CoverPreview { get { return coverPreview; } }

        public Label CountLabel { get { return countLabel; } }
        public Button NextButton { get { return nextButton; } }

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
        ListView ListView { get; }

        Label FileLabel { get; }
        Label AlbumLabel { get; }

        PictureBox CoverPreview { get; }

        Label CountLabel { get; }
        Button NextButton { get; }

        BackgroundWorker BackgroundWorker { get; }

        ToolStripProgressBar SearchProgressBar { get; }
        ToolStripStatusLabel StatusLabel { get; }

        void CloseDialog();
    }
}