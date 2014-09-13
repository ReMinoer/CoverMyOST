using System.Drawing;
using NUnit.Framework;
using TagLib;

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
            MusicFile file = client.Files[TestPaths.MusicA];
            file.Album = name;
            file.Save();

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

            MusicFile file = client.Files[TestPaths.MusicB];
            Image cover = Image.FromFile(TestPaths.CoverA);
            file.Cover = cover;
            file.Save();

            // Test
            var result = new MusicFile(TestPaths.MusicB);
            Assert.AreEqual(result.Cover.PhysicalDimension, cover.PhysicalDimension);
        }
    }
}