using CoverMyOST.Models;
using CoverMyOST.Models.MusicPlayers;
using CoverMyOST.Models.Wizards;
using CoverMyOST.Windows.Models;
using CoverMyOST.Windows.Views;

namespace CoverMyOST.Windows.Dialogs
{
    public class CoverSeriesWizardDialog
    {
        public CoverSeriesWizardModel Model { get; private set; }
        public IMusicPlayerModel MusicPlayer { get; private set; }
        public CoverSeriesWizardView View { get; private set; }

        public CoverSeriesWizardDialog(CoverMyOSTClient coverMyOSTClient)
        {
            Model = new CoverSeriesWizardModel(coverMyOSTClient);
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
                View.SearchProgress((CoverSearchProgress)args.UserState, args.ProgressPercentage);
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
                View.SearchEnd(Model.State != CoverSearchState.Cancel);
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
            View.Initialize(Model.FilePath, Model.SelectedCover, Model.EditedAlbum, Model.FileIndex, Model.FilesCount);
        }
    }
}