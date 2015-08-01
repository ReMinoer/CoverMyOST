using System;
using System.Diagnostics;

namespace CoverMyOST.Models.MusicPlayers
{
    public class DefaultMusicPlayerModel : IMusicPlayerModel
    {
        private string _lastPath;
        private Process _musicProcess;
        public bool IsPlaying { get; private set; }

        public event EventHandler MusicEnded
        {
            add { _musicProcess.Exited += value; }
            remove { _musicProcess.Exited -= value; }
        }

        public DefaultMusicPlayerModel()
        {
            _musicProcess.Exited += (sender, args) => IsPlaying = false;
        }

        public void Play(string path)
        {
            if (path == _lastPath && IsPlaying)
                return;

            _musicProcess = Process.Start(path);
            _lastPath = path;
            IsPlaying = true;
        }

        public void Stop()
        {
            if (_musicProcess != null && !_musicProcess.HasExited)
                _musicProcess.Kill();
            IsPlaying = false;
        }

        public void Toggle(string path)
        {
            if (IsPlaying)
                Stop();
            else
                Play(path);
        }
    }
}