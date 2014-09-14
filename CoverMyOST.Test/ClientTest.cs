using System.Drawing;
using CoverMyOST.Test.Helpers;
using NUnit.Framework;
using TagLib;
using File = TagLib.File;

namespace CoverMyOST.Test
{
    internal class ClientTest
    {
        [Test]
        public void ChangeDirectory()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            // Test
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicA));
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicB));
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicC));
        }

        [Test]
        public void EditAlbumTag()
        {
            // Prerequisites
            File temp = File.Create(TestPaths.MusicA);
            temp.Tag.Album = "";
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            const string name = "Insert an album name here";
            client.Files[TestPaths.MusicA].Album = name;
            client.Files[TestPaths.MusicA].Save();

            // Test
            var result = new MusicFile(TestPaths.MusicA);
            Assert.AreEqual(result.Album, name);
        }

        [Test]
        public void EditCover()
        {
            // Prerequisites
            File temp = File.Create(TestPaths.MusicB);
            temp.Tag.Pictures = new IPicture[0];
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            var cover = new Bitmap(Image.FromFile(TestPaths.CoverA));
            client.Files[TestPaths.MusicB].Cover = cover;
            client.Files[TestPaths.MusicB].Save();

            // Test
            var result = new MusicFile(TestPaths.MusicB).Cover;
            Assert.AreEqual(result.Size, cover.Size);
        }
    }
}