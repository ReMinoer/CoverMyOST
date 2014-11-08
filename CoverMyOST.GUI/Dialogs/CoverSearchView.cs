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

        public Label TitleLabel { get { return titleLabel; } }
        public Label AlbumLabel { get { return albumLabel; } }

        public PictureBox CoverPreview { get { return coverPreview; } }

        public Label CountLabel { get { return countLabel; } }
        public Button NextButton { get { return nextButton; } }

        public BackgroundWorker BackgroundWorker { get { return backgroundWorker; } }
    }

    public interface ICoverSearchView
    {
        ListView ListView { get; }

        Label TitleLabel { get; }
        Label AlbumLabel { get; }

        PictureBox CoverPreview { get; }

        Label CountLabel { get; }
        Button NextButton { get; }

        BackgroundWorker BackgroundWorker { get; }
    }
}