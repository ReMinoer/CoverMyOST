using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CoverMyOST.Test.Helpers;
using NUnit.Framework;
using TagLib;
using File = TagLib.File;

namespace CoverMyOST.Test
{
    internal class ClientTest
    {
        [Test]
        public void AddFile()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.AddFile(TestPaths.MusicA);

            // Test
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicA));
        }

        [Test]
        public void AddDirectory()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.AddDirectory(TestPaths.MusicDirectory);

            // Test
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicA));
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicB));
            Assert.IsFalse(client.Files.ContainsKey(TestPaths.MusicC));
        }

        [Test]
        public void AddDirectoryWithRecursivity()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.AddDirectory(TestPaths.MusicDirectory, true);

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
            client.AddFile(TestPaths.MusicA);

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
            client.AddFile(TestPaths.MusicB);

            var cover = new Bitmap(Image.FromFile(TestPaths.CoverA));
            client.Files[TestPaths.MusicB].Cover = cover;
            client.Files[TestPaths.MusicB].Save();

            // Test
            var result = new MusicFile(TestPaths.MusicB).Cover;

            var memoryStream = new MemoryStream();
            result.Save(memoryStream, ImageFormat.Jpeg);
            byte[] resultBytes = memoryStream.ToArray();

            memoryStream = new MemoryStream();
            result.Save(memoryStream, ImageFormat.Jpeg);
            byte[] coverBytes = memoryStream.ToArray();

            Assert.AreEqual(resultBytes, coverBytes);
        }
    }
}