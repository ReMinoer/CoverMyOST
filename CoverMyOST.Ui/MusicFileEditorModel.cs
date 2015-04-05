using System.Drawing;

namespace CoverMyOST.Ui
{
    public class MusicFileEditorModel
    {
        private MusicFile _file;

        public MusicFile File
        {
            get { return _file; }
            set
            {
                _file = value;
                ResetAll();
            }
        }

        public string EditedAlbum { get; set; }
        public Bitmap SelectedCover { get; set; }

        public MusicFileEditorModel(MusicFile file)
        {
            _file = file;
            ResetAll();
        }

        public void ResetAll()
        {
            ResetAlbum();
            ResetCover();
        }

        public void ResetAlbum()
        {
            EditedAlbum = File.Album;
        }

        public void ResetCover()
        {
            SelectedCover = File.Cover;
        }

        public void Apply()
        {
            File.Album = EditedAlbum;
            File.Cover = SelectedCover;
        }
    }
}