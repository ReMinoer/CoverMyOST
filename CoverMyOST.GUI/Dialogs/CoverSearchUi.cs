using System;
using System.ComponentModel;
using System.Drawing;
using Diese.UserInterface;

namespace CoverMyOST.GUI.Dialogs
{
    public class CoverSearchUi : IUserInterface<CoverSearchUi.IModel, CoverSearchUi.IView>
    {
        public IModel Model { get; private set; }
        public IView View { get; private set; }

        public CoverSearchUi(IModel model, IView view)
        {
            Model = model;
            View = view;

            BindEvents(Model, View);
        }

        public void BindEvents(IModel model, IView view)
        {
            Model.Initialize += View.OnInitialize;

            View.ApplyRequest += Model.OnApplyRequest;

            View.EditAlbumRequest += Model.OnEditAlbumRequest;
            View.ResetAlbumRequest += Model.OnResetAlbumRequest;
            Model.ResetAlbum += View.OnResetAlbum;

            View.CoverSelectionRequest += Model.OnCoverSelectionRequest;
            Model.CoverChange += View.OnCoverChange;

            Model.SearchProgress += View.OnSearchProgress;
            Model.SearchCancel += View.OnSearchCancel;
            Model.SearchError += View.OnSearchError;
            Model.SearchComplete += View.OnSearchComplete;
            Model.SearchEnd += View.OnSearchEnd;

            View.ToggleSongRequest += Model.OnToggleSongRequest;
            Model.ToggleSong += View.OnToggleSong;

            View.CloseRequest += Model.OnCloseRequest;
            Model.Close += View.OnClose;

            Model.Reset();
        }

        public interface IModel
        {
            event EventHandler<InitializeEventArgs> Initialize;
            event EventHandler<ResetAlbumEventArgs> ResetAlbum;
            event EventHandler<CoverChangeEventArgs> CoverChange;
            event EventHandler<ProgressChangedEventArgs> SearchProgress;
            event EventHandler SearchCancel;
            event EventHandler<SearchErrorEventArgs> SearchError;
            event EventHandler SearchComplete;
            event EventHandler<SearchEndEventArgs> SearchEnd;
            event EventHandler<ToggleSongEventArgs> ToggleSong;
            event EventHandler Close;
            void Reset();
            void OnEditAlbumRequest(object sender, EditAlbumRequestEventArgs e);
            void OnResetAlbumRequest(object sender, EventArgs e);
            void OnCoverSelectionRequest(object sender, CoverSelectionEventArgs e);
            void OnApplyRequest(object sender, EventArgs e);
            void OnToggleSongRequest(object sender, EventArgs e);
            void OnCloseRequest(object sender, EventArgs e);
        }

        public interface IView
        {
            event EventHandler<EditAlbumRequestEventArgs> EditAlbumRequest;
            event EventHandler ResetAlbumRequest;
            event EventHandler<CoverSelectionEventArgs> CoverSelectionRequest;
            event EventHandler ApplyRequest;
            event EventHandler ToggleSongRequest;
            event EventHandler CloseRequest;
            void OnInitialize(object sender, InitializeEventArgs e);
            void OnResetAlbum(object sender, ResetAlbumEventArgs e);
            void OnCoverChange(object sender, CoverChangeEventArgs e);
            void OnSearchProgress(object sender, ProgressChangedEventArgs e);
            void OnSearchCancel(object sender, EventArgs e);
            void OnSearchError(object sender, SearchErrorEventArgs e);
            void OnSearchComplete(object sender, EventArgs e);
            void OnSearchEnd(object sender, SearchEndEventArgs e);
            void OnToggleSong(object sender, ToggleSongEventArgs e);
            void OnClose(object sender, EventArgs e);
        }

        public class EditAlbumRequestEventArgs : EventArgs
        {
            public string AlbumName { get; set; }
        }

        public class ResetAlbumEventArgs : EventArgs
        {
            public string DefaultAlbumName { get; set; }
        }

        public class SearchErrorEventArgs : EventArgs
        {
            public string ErrorMessage { get; set; }
        }

        public class SearchEndEventArgs : EventArgs
        {
            public bool EnableForm { get; set; }
        }

        public class ToggleSongEventArgs : EventArgs
        {
            public bool ToggleSong { get; set; }
        }

        public class InitializeEventArgs : EventArgs
        {
            public int FileIndex { get; set; }
            public int FilesCount { get; set; }
            public MusicFile CurrentFile { get; set; }
        }

        public class CoverSelectionEventArgs : EventArgs
        {
            public int Index { get; set; }
            public string Group { get; set; }
        }

        public class CoverChangeEventArgs : EventArgs
        {
            public Image Cover { get; set; }
            public string Name { get; set; }
        }
    }
}