using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoverMyOST.Test.Content;
using NUnit.Framework;

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
            Assert.AreEqual(client.WorkingDirectory, TestPaths.MusicDirectory);
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicA));
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicB));
            Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicC));
        }

		[Test]
		public void FilterFiles()
		{
			// Prerequisites
			ResetFile(TestPaths.MusicA);
			ResetFile(TestPaths.MusicB);
			ResetFile(TestPaths.MusicC);

			var temp = new MusicFile(TestPaths.MusicA)
			{
				Album = "Death",
				Cover = new Bitmap(Image.FromFile(TestPaths.CoverA))
			};
			temp.Save();

			var temp2 = new MusicFile(TestPaths.MusicB)
			{
				Album = "Angel",
			};
			temp2.Save();

			// Process 1
			var client = new CoverMyOSTClient();
			client.ChangeDirectory(TestPaths.MusicDirectory);

			client.Filter = MusicFileFilter.NoAlbum;

			// Test 1
			Assert.IsFalse(client.Files.ContainsKey(TestPaths.MusicA));
			Assert.IsFalse(client.Files.ContainsKey(TestPaths.MusicB));
			Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicC));

			// Process 2
			client.Filter = MusicFileFilter.NoCover;

			// Test 2
			Assert.IsFalse(client.Files.ContainsKey(TestPaths.MusicA));
			Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicB));
			Assert.IsTrue(client.Files.ContainsKey(TestPaths.MusicC));
		}

        [Test]
        public void EditAlbumTag()
        {
            // Prerequisites
            string filePath = TestPaths.MusicA;
            ResetFile(filePath);

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            const string name = "Insert an album name here";
            client.Files[filePath].Album = name;
            client.Files[filePath].Save();

            // Test
            var result = new MusicFile(filePath);
            Assert.AreEqual(result.Album, name);
        }

        [Test]
        public void EditCover()
        {
            // Prerequisites
            string filePath = TestPaths.MusicB;
            ResetFile(filePath);

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            var cover = new Bitmap(Image.FromFile(TestPaths.CoverA));
            client.Files[filePath].Cover = cover;
            client.Files[filePath].Save();

            // Test
            var result = new MusicFile(filePath);
            Assert.AreEqual(result.Cover.Size, cover.Size);
        }

        [Test]
        public void SaveAll()
        {
            // Prerequisites
            ResetFile(TestPaths.MusicA);
            ResetFile(TestPaths.MusicB);
            ResetFile(TestPaths.MusicC);

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            const string albumA = "AlbumA";
            const string albumB = "AlbumB";
            const string albumC = "AlbumC";
            client.Files[TestPaths.MusicA].Album = albumA;
            client.Files[TestPaths.MusicB].Album = albumB;
            client.Files[TestPaths.MusicC].Album = albumC;

            client.SaveAll();

            // Test
            var resultA = new MusicFile(TestPaths.MusicA);
            var resultB = new MusicFile(TestPaths.MusicB);
            var resultC = new MusicFile(TestPaths.MusicC);
            Assert.AreEqual(resultA.Album, albumA);
            Assert.AreEqual(resultB.Album, albumB);
            Assert.AreEqual(resultC.Album, albumC);
        }

        [Test]
        public void SearchCover()
        {
            // Prerequisites
            string filePath = TestPaths.MusicA;
            ResetFile(filePath);
            var temp = new MusicFile(filePath) {Album = "Death"};
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);
			client.Galleries.EnableAll();

            Dictionary<string, Bitmap> covers = client.SearchCover(filePath);

            // Test
            Assert.IsTrue(covers.Count > 0);
        }
			
		[Test]
		public void SearchCoverWithFilterOnGalleries()
		{
			// Prerequisites
			string filePath = TestPaths.MusicB;
			ResetFile(filePath);
			var temp = new MusicFile(filePath) {Album = "cover"};
			temp.Save();

			// Process 1
			var client = new CoverMyOSTClient();
			client.ChangeDirectory(TestPaths.MusicDirectory);
			client.Galleries.AddLocalGallery(TestPaths.CoverDirectory);
			client.Galleries.AddLocalGallery(TestPaths.CoverDirectoryBis);

			client.Galleries.DisableAll();
			client.Galleries[TestPaths.CoverDirectory].Enable = true;

			Dictionary<string, Bitmap> covers = client.SearchCover(filePath);

			// Test 1
			Assert.IsTrue(covers.ContainsKey(TestPaths.CoverA));
			Assert.IsFalse(covers.ContainsKey(TestPaths.CoverB));

			// Process 2
			client.Galleries.EnableAll();
			covers = client.SearchCover(filePath);

			// Test 2
			Assert.IsTrue(covers.ContainsKey(TestPaths.CoverA));
			Assert.IsTrue(covers.ContainsKey(TestPaths.CoverB));
		}

		static public void AssignCoverFromOnlineGallery<TOnlineGallery>(string filePath, string query)
			where TOnlineGallery : IOnlineGallery, new()
        {
            // Prerequisites
            ResetFile(filePath);
            var temp = new MusicFile(filePath) {Album = query};
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

			Dictionary<string, Bitmap> covers = client.SearchCover<TOnlineGallery>(filePath);

            client.Files[filePath].Cover = covers.Values.First();
            client.Files[filePath].Save();

            // Test
            Assert.IsTrue(covers.Count > 0);
        }

        static public void ResetFile(string path)
        {
            var file = new MusicFile(path) {Album = "", Cover = null};
            file.Save();
        }
    }
}