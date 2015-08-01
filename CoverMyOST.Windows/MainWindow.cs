using System.IO;
using CoverMyOST.Models.MusicPlayers;
using CoverMyOST.Windows.Dialogs;
using CoverMyOST.Windows.Models;
using CoverMyOST.Windows.Views;

namespace CoverMyOST.Windows
{
    public class MainWindow
    {
        public MainModel Model { get; private set; }
        public IMusicPlayerModel MusicPlayer { get; private set; }
        public MainView View { get; private set; }

        public MainWindow()
        {
            Model = new MainModel();
            View = new MainView(Model.Client.Files.Values);

#if !MONO
            MusicPlayer = new WindowsMediaPlayerModel();
#else
            MusicPlayer = new DefaultMusicPlayerModel();
#endif

            // Handle files

            View.ChangeFolderRequest += (sender, args) =>
            {
                MusicPlayer.Stop();
                Model.ChangeFolder(args.FolderPath);
                View.ResetView(Model.Client.Files.Values);
                View.ShowCountsInStatusStrip(Model.FilesCount, Model.SelectedFilesCount, Model.DisplayedFilesCount);
                View.CompleteFolderChange();
            };

            View.SaveAllRequest += (sender, args) =>
            {
                Model.SaveConfiguration();
                Model.SaveAll();
                View.CompleteSaveAll();
            };

            View.ChangeFilterRequest += (sender, args) =>
            {
                Model.ChangeFilter(args.Filter);
                View.RefreshGrid(Model.Client.Files.Values);
                View.ShowCountsInStatusStrip(Model.FilesCount, Model.SelectedFilesCount, Model.DisplayedFilesCount);
            };

            // Edit files

            View.SelectedFilesChanged += (sender, args) =>
            {
                string path = Path.Combine(Model.Client.WorkingDirectory, args.Filename);
                Model.ChangeFileSelection(path, args.IsSelected);
                View.ShowCountsInStatusStrip(Model.FilesCount, Model.SelectedFilesCount, Model.DisplayedFilesCount);
            };

            View.AlbumNameChanged += (sender, args) =>
            {
                string path = Path.Combine(Model.Client.WorkingDirectory, args.Filename);
                Model.ChangeFileAlbumName(path, args.AlbumName);
                View.HighlightAlbumChange(args.Filename);
                View.SetAsEdited();
            };

            // Dialogs

            View.ShowGalleryManagerRequest += (sender, args) =>
            {
                var galleryManagerUi = new GalleryManagerDialog();
                galleryManagerUi.View.ShowDialog();
            };

            View.ShowCoverSeriesWizardRequest += (sender, args) =>
            {
                MusicPlayer.Stop();

                var coverSeriesWizardDialog = new CoverSeriesWizardDialog(Model.Client);
                coverSeriesWizardDialog.View.ShowDialog();

                View.RefreshGrid(Model.Client.Files.Values);
                View.ShowCountsInStatusStrip(Model.FilesCount, Model.SelectedFilesCount, Model.DisplayedFilesCount);

                Model.SetAsEdited();
                View.SetAsEdited();
            };

            // Music player

            View.PlayMusicRequest += (sender, args) =>
            {
                string path = Path.Combine(Model.Client.WorkingDirectory, args.Filename);
                MusicPlayer.Play(path);
                View.CompleteMusicPlay(args.Filename);
            };

            View.StopMusicRequest += (sender, args) =>
            {
                View.DisableStopButton();
                MusicPlayer.Stop();
                View.ShowCountsInStatusStrip(Model.FilesCount, Model.SelectedFilesCount, Model.DisplayedFilesCount);
            };

            MusicPlayer.MusicEnded += (sender, args) =>
            {
                View.DisableStopButton();
                View.ShowCountsInStatusStrip(Model.FilesCount, Model.SelectedFilesCount, Model.DisplayedFilesCount);
            };

            // Closing

            View.CloseRequest += (sender, args) =>
            {
                Model.SaveConfiguration();

                if (args.SaveRequested)
                    Model.SaveAll();
            };
        }
    }
}