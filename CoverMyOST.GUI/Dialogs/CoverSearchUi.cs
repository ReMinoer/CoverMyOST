using System;
using System.ComponentModel;
using System.Drawing;

namespace CoverMyOST.GUI.Dialogs
{
    public class CoverSearchUi
    {
        public IModel Model { get; set; }
        public IView View { get; set; }

        public CoverSearchUi(IModel model, IView view)
        {
            Model = model;
            View = view;

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

            View.PlaySongRequest += Model.OnPlaySongRequest;
            Model.PlaySong += View.OnPlaySong;

            View.CloseRequest += Model.OnCloseRequest;
            Model.Close += View.OnClose;

            Model.Reset();
        }

        public void Show(bool stayFocus = false)
        {
            View.Show(stayFocus);
        }

        public interface IModel
        {
            event EventHandler<InitializeEventArgs> Initialize;
            event EventHandler<string> ResetAlbum;
            event EventHandler<CoverChangeEventArgs> CoverChange;
            event EventHandler<ProgressChangedEventArgs> SearchProgress;
            event EventHandler SearchCancel;
            event EventHandler<string> SearchError;
            event EventHandler SearchComplete;
            event EventHandler<bool> SearchEnd;
            event EventHandler<bool> PlaySong;
            event EventHandler Close;
            void Reset();
            void OnEditAlbumRequest(object sender, string albumName);
            void OnResetAlbumRequest(object sender, EventArgs e);
            void OnCoverSelectionRequest(object sender, CoverSelectionEventArgs e);
            void OnApplyRequest(object sender, EventArgs e);
            void OnPlaySongRequest(object sender, EventArgs e);
            void OnCloseRequest(object sender, EventArgs e);
        }

        public interface IView
        {
            event EventHandler<string> EditAlbumRequest;
            event EventHandler ResetAlbumRequest;
            event EventHandler<CoverSelectionEventArgs> CoverSelectionRequest;
            event EventHandler ApplyRequest;
            event EventHandler PlaySongRequest;
            event EventHandler CloseRequest;
            void Show(bool stayFocus);
            void OnInitialize(object sender, InitializeEventArgs e);
            void OnResetAlbum(object sender, string albumName);
            void OnCoverChange(object sender, CoverChangeEventArgs e);
            void OnSearchProgress(object sender, ProgressChangedEventArgs e);
            void OnSearchCancel(object sender, EventArgs e);
            void OnSearchError(object sender, string errorMessage);
            void OnSearchComplete(object sender, EventArgs e);
            void OnSearchEnd(object sender, bool enableForm);
            void OnPlaySong(object sender, bool isPlaying);
            void OnClose(object sender, EventArgs e);
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