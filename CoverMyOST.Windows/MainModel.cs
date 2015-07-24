namespace CoverMyOST.Windows
{
    public class MainModel
    {
        public CoverMyOSTClient Client { get; private set; }
        private bool _isSaved = true;

        public int FilesCount { get { return Client.AllFiles.Count; } }
        public int SelectedFilesCount { get { return Client.AllSelectedFiles.Count; } }
        public int DisplayedFilesCount { get { return Client.Files.Count; } }

        public MainModel()
        {
            Client = new CoverMyOSTClient();
            Client.LoadConfiguration();
        }

        public void ChangeFolder(string folderPath)
        {
            Client.ChangeDirectory(folderPath);
            Client.SaveConfiguration();
        }

        public void SaveAll()
        {
            Client.SaveAll();

            _isSaved = true;
        }

        public void SaveConfiguration()
        {
            Client.SaveConfiguration();
        }

        public void ChangeFilter(MusicFileFilter musicFileFilter)
        {
            Client.Filter = musicFileFilter;
        }

        public void ChangeFileSelection(string path, bool isSelected)
        {
            Client.Files[path].Selected = isSelected;
        }

        public void ChangeFileAlbumName(string path, string albumName)
        {
            if (Client.Files[path].Album != albumName)
                Client.Files[path].Album = albumName;
        }

        public void SetAsEdited()
        {
            _isSaved = false;
        }
    }
}