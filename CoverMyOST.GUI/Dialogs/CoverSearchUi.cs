namespace CoverMyOST.GUI.Dialogs
{
    public class CoverSearchUi
    {
        public CoverSearchModel Model { get; private set; }
        public CoverSearchView View { get; private set; }

        public CoverSearchUi(CoverMyOSTClient coverMyOSTClient)
        {
            Model = new CoverSearchModel(coverMyOSTClient);
            View = new CoverSearchView();

            // Initialization

            Model.Initialize += (sender, args) =>
            {
                View.Initialize(Model.CurrentFile, Model.FileIndex, Model.FilesCount);
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
                View.ChangeAlbumName(Model.CurrentFile.Album);
            };

            View.CoverSelectionRequest += (sender, args) =>
            {
                switch (args.Action)
                {
                    case CoverSearchView.CoverSelectionAction.Remove:
                        Model.RemoveCoverSelected();
                        break;
                    case CoverSearchView.CoverSelectionAction.Reset:
                        Model.ResetCoverSelected();
                        break;
                    case CoverSearchView.CoverSelectionAction.Change:
                        Model.ChangeCoverSelected(args.SelectionIndex);
                        break;
                }

                View.ChangeCover(Model.CoverSelected);
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

            Model.SearchComplete += (sender, args) =>
            {
                View.SearchComplete();
            };

            Model.SearchEnd += (sender, args) =>
            {
                View.SearchEnd(Model.Step != CoverSearchStep.Cancel);
            };

            // Music player

            View.ToggleSongRequest += (sender, args) =>
            {
                Model.ToggleSong();
                View.UpdatePlayMusicButton(Model.IsPlayingSong);
            };

            // Close form

            Model.ProcessEnd += (sender, args) =>
            {
                View.Close();
            };

            View.Closing += (sender, args) =>
            {
                Model.Close();
            };

            Model.Reset();
            View.Initialize(Model.CurrentFile, Model.FileIndex, Model.FilesCount);
        }
    }
}