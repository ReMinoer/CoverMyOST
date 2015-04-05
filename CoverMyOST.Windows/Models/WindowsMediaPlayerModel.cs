using System;
using System.Windows.Media;
using CoverMyOST.Ui;

namespace CoverMyOST.Windows.Models
{
    public class WindowsMediaPlayerModel : IMusicPlayerModel
    {
        private readonly MediaPlayer _mediaPlayer;
        public bool IsPlaying { get; private set; }

        public event EventHandler MusicEnded
        {
            add { _mediaPlayer.MediaEnded += value; }
            remove { _mediaPlayer.MediaEnded -= value; }
        }

        public WindowsMediaPlayerModel()
        {
            _mediaPlayer = new MediaPlayer();
            IsPlaying = false;

            _mediaPlayer.MediaEnded += (sender, args) => Stop();
        }

        public void Play(string path)
        {
            _mediaPlayer.Close();
            _mediaPlayer.Open(new Uri(path));
            _mediaPlayer.Play();
            IsPlaying = true;
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
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