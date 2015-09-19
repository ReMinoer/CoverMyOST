using System.Drawing;

namespace CoverMyOST.Editors
{
    public class MusicFileEditor
    {
        private MusicFile _file;
        public string EditedAlbum { get; set; }
        public Bitmap SelectedCover { get; set; }

        public MusicFile File
        {
            get { return _file; }
            set
            {
                _file = value;
                ResetAllTags();
            }
        }

        public MusicFileEditor(MusicFile file)
        {
            _file = file;
            ResetAllTags();
        }

        public void ResetAllTags()
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
            File.Save();
        }
    }
}