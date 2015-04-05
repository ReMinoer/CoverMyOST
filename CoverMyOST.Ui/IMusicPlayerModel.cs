using System;

namespace CoverMyOST.Ui
{
    public interface IMusicPlayerModel
    {
        event EventHandler MusicEnded;

        bool IsPlaying { get; }
        void Play(string url);
        void Stop();
        void Toggle(string path);
    }
}