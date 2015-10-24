using System.IO;
using System.Linq;
using CoverMyOST.Editors;
using CoverMyOST.FileManagers;
using CoverMyOST.MusicPlayers;
using CoverMyOST.Windows.Dialogs;
using CoverMyOST.Windows.MusicPlayers;

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
            View = new MainView();

#if !MONO
            MusicPlayer = new WindowsMediaPlayerModel();
#else
            MusicPlayer = new DefaultMusicPlayerModel();
#endif

            View.ResetView(Model.Files, Model.SelectedFiles);
            View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());

            // Handle files

            View.ChangeFolderRequest += (sender, args) =>
            {
                MusicPlayer.Stop();
                Model.ChangeFolder(args.FolderPath);
                Model.ChangeAllFilesSelection(true);
                Model.ChangeFilter(MusicFileFilter.None);
                View.RefreshGrid(Model.DisplayedFiles, Model.SelectedFiles);
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());
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
                View.RefreshGrid(Model.DisplayedFiles, Model.SelectedFiles);
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());
            };

            // Edit files

            View.SelectedFilesChanged += (sender, args) =>
            {
                string path = Path.Combine(Model.WorkingDirectory, args.Filename);
                Model.ChangeFileSelection(path, args.IsSelected);
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());
            };

            View.AlbumNameChanged += (sender, args) =>
            {
                string path = Path.Combine(Model.WorkingDirectory, args.Filename);
                Model.ChangeFileAlbumName(path, args.AlbumName);
                View.HighlightAlbumChange(args.Filename);
                View.SetAsEdited();
            };

            View.IndividualWizardRequest += (sender, args) =>
            {
                MusicPlayer.Stop();

                var editor = new MusicFileCollectionEditor(Model.MusicFileCollectionEditor[Model[args.Filename].Path]);

                var coverSeriesWizardDialog = new CoverSeriesWizardDialog(editor, Model.GalleryManager);
                coverSeriesWizardDialog.View.ShowDialog();

                View.RefreshGrid(Model.DisplayedFiles, Model.SelectedFiles);
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());

                View.SetAsEdited();
            };

            // Dialogs

            View.ShowGalleryManagerRequest += (sender, args) =>
            {
                var galleryManagerUi = new GalleryManagerDialog(Model.GalleryManager);
                galleryManagerUi.View.ShowDialog();
            };

            View.ShowCoverSeriesWizardRequest += (sender, args) =>
            {
                MusicPlayer.Stop();

                var coverSeriesWizardDialog = new CoverSeriesWizardDialog(Model.MusicFileCollectionEditor, Model.GalleryManager);
                coverSeriesWizardDialog.View.ShowDialog();

                View.RefreshGrid(Model.DisplayedFiles, Model.SelectedFiles);
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());

                View.SetAsEdited();
            };

            // Music player

            View.PlayMusicRequest += (sender, args) =>
            {
                string path = Path.Combine(Model.WorkingDirectory, args.Filename);
                MusicPlayer.Play(path);
                View.CompleteMusicPlay(args.Filename);
            };

            View.StopMusicRequest += (sender, args) =>
            {
                View.DisableStopButton();
                MusicPlayer.Stop();
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());
            };

            MusicPlayer.MusicEnded += (sender, args) =>
            {
                View.DisableStopButton();
                View.ShowCountsInStatusStrip(Model.Files.Count(), Model.SelectedFiles.Count(), Model.DisplayedFiles.Count());
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