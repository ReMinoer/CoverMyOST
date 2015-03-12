using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using TagLib;
using TagLib.Id3v2;
using File = TagLib.File;

namespace CoverMyOST
{
    public class MusicFile
    {
        private readonly File _file;
        private readonly string[] _musicExtensions = {".mp3", ".wma", ".flac", ".ogg"};
        public string Path { get; private set; }

        public Bitmap Cover
        {
            get
            {
                if (_file.Tag.Pictures.All(p => p.Type != PictureType.FrontCover))
                    return null;

                IPicture picture = _file.Tag.Pictures.First(p => p.Type == PictureType.FrontCover);

                var memoryStream = new MemoryStream(picture.Data.Data);
                return new Bitmap(Image.FromStream(memoryStream));
            }
            set
            {
                if (value == null)
                {
                    _file.Tag.Pictures = new IPicture[0];
                    return;
                }

                var memoryStream = new MemoryStream();
                value.Save(memoryStream, ImageFormat.Jpeg);

                byte[] imageBytes = memoryStream.ToArray();
                var byteVector = new ByteVector(imageBytes, imageBytes.Length);

                var picture = new Picture(byteVector);
                var albumCoverPictFrame = new AttachedPictureFrame(picture)
                {
                    MimeType = MediaTypeNames.Image.Jpeg,
                    Type = PictureType.FrontCover
                };

                _file.Tag.Pictures = new IPicture[] {albumCoverPictFrame};
            }
        }

        public string Album
        {
            get { return _file.Tag.Album; }
            set { _file.Tag.Album = value; }
        }

        internal MusicFile(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            if (extension == null || !_musicExtensions.Contains(extension.ToLower()))
                throw new UnsupportedFormatException();

            _file = File.Create(path);
            Path = path;
        }

        public void Save()
        {
            _file.Save();
        }
    }
}