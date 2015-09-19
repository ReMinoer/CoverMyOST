using System;

namespace CoverMyOST.MusicPlayers
{
    public interface IMusicPlayerModel
    {
        bool IsPlaying { get; }
        event EventHandler MusicEnded;
        void Play(string url);
        void Stop();
        void Toggle(string path);
    }
}