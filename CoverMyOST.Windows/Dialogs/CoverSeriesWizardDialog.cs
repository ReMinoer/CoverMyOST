using CoverMyOST.Editors;
using CoverMyOST.Galleries;
using CoverMyOST.MusicPlayers;
using CoverMyOST.Windows.Dialogs.Views;
using CoverMyOST.Windows.MusicPlayers;
using CoverMyOST.Wizards;

namespace CoverMyOST.Windows.Dialogs
{
    public class CoverSeriesWizardDialog
    {
        public CoverSeriesWizard Model { get; private set; }
        public IMusicPlayerModel MusicPlayer { get; private set; }
        public CoverSeriesWizardView View { get; private set; }

        public CoverSeriesWizardDialog(MusicFileCollectionEditor musicFileEditors, GalleryManager galleryManager)
        {
            Model = new CoverSeriesWizard(musicFileEditors, galleryManager);
            View = new CoverSeriesWizardView();

#if !MONO
            MusicPlayer = new WindowsMediaPlayerModel();
#else
            MusicPlayer = new DefaultMusicPlayerModel();
#endif

            // Initialization

            Model.Initialize += (sender, args) =>
            {
                if (MusicPlayer.IsPlaying)
                    MusicPlayer.Play(Model.FilePath);
                View.Initialize(Model.FilePath, Model.SelectedCover, Model.EditedAlbum, Model.FileIndex, Model.FilesCount);
            };

            // Edition

            View.ApplyRequest += (sender, args) =>
            {
                Model.Apply();
            };

            View.EditAlbumRequest += (sender, args) =>
            {
                Model.EditAlbum(args.AlbumName);
            };

            View.ResetAlbumRequest += (sender, args) =>
            {
                View.ChangeAlbumName(Model.EditedAlbum);
            };

            View.CoverSelectionRequest += (sender, args) =>
            {
                switch (args.Action)
                {
                    case CoverSeriesWizardView.CoverSelectionAction.Remove:
                        Model.RemoveCoverSelected();
                        break;
                    case CoverSeriesWizardView.CoverSelectionAction.Reset:
                        Model.ResetCoverSelected();
                        break;
                    case CoverSeriesWizardView.CoverSelectionAction.Change:
                        Model.ChangeCoverSelected(args.SelectionIndex);
                        break;
                }

                View.ChangeCover(Model.SelectedCover);
            };

            // Search state

            Model.SearchProgress += (sender, args) =>
            {
                View.SearchProgress(args.Status);
            };

            Model.SearchCancel += (sender, args) =>
            {
                View.SearchCancel();
            };

            Model.SearchError += (sender, args) =>
            {
                View.SearchError(args.ErrorMessage);
            };

            Model.SearchSuccess += (sender, args) =>
            {
                View.SearchSuccess();
            };

            Model.SearchEnd += (sender, args) =>
            {
                View.SearchEnd(Model.State);
            };

            // Music player

            View.ToggleSongRequest += (sender, args) =>
            {
                MusicPlayer.Toggle(Model.FilePath);
                View.UpdatePlayMusicButton(MusicPlayer.IsPlaying);
            };

            MusicPlayer.MusicEnded += (sender, args) =>
            {
                View.UpdatePlayMusicButton(MusicPlayer.IsPlaying);
            };

            // Close form

            Model.ProcessEnd += (sender, args) =>
            {
                View.Close();
            };

            View.Closing += (sender, args) =>
            {
                Model.Close();
                MusicPlayer.Stop();
            };

            Model.ResetSearch();
        }
    }
}