using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using File = TagLib.File;

namespace CoverMyOST.Test
{
    public class ClientTest
    {
        static public readonly string TestDirectory = Path.GetFullPath("Files/");
        static public readonly string TestPathA = Path.GetFullPath("Files/testA.mp3");
        static public readonly string TestPathB = Path.GetFullPath("Files/testB.mp3");
        static public readonly string TestPathC = Path.GetFullPath("Files/dir/testC.mp3");

        [Test]
        public void AddFile()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.AddFile(TestPathA);

            // Test
            Assert.IsTrue(client.Files.ContainsKey(TestPathA));
        }

        [Test]
        public void AddDirectory()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.AddDirectory(TestDirectory);

            // Test
            Assert.IsTrue(client.Files.ContainsKey(TestPathA));
            Assert.IsTrue(client.Files.ContainsKey(TestPathB));
            Assert.IsFalse(client.Files.ContainsKey(TestPathC));
        }

        [Test]
        public void AddDirectoryWithRecursivity()
        {
            // Process
            var client = new CoverMyOSTClient();
            client.AddDirectory(TestDirectory, true);

            // Test
            Assert.IsTrue(client.Files.ContainsKey(TestPathA));
            Assert.IsTrue(client.Files.ContainsKey(TestPathB));
            Assert.IsTrue(client.Files.ContainsKey(TestPathC));
        }

        [Test]
        public void EditAlbumTag()
        {
            // Prerequisites
            File temp = File.Create(TestPathA);
            temp.Tag.Album = "";
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.AddFile(TestPathA);

            const string name = "Insert an album name here";
            MusicFile file = client.Files[TestPathA];
            file.Album = name;
            file.Save();

            // Test
            File result = File.Create(TestPathA);
            Assert.AreEqual(result.Tag.Album, name);
        }
    }
}