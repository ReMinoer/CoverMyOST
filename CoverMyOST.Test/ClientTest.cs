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
            var temp = ResetFile(TestPaths.MusicA);
            var temp2 = ResetFile(TestPaths.MusicB);
            ResetFile(TestPaths.MusicC);

            temp.Album = "Death";
            temp.Cover = new Bitmap(Image.FromFile(TestPaths.CoverA));
            temp.Save();

            temp2.Album = "Angel";
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
            var result = LoadFile(filePath);
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
            var result = LoadFile(filePath);
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
            var resultA = LoadFile(TestPaths.MusicA);
            var resultB = LoadFile(TestPaths.MusicB);
            var resultC = LoadFile(TestPaths.MusicC);
            Assert.AreEqual(resultA.Album, albumA);
            Assert.AreEqual(resultB.Album, albumB);
            Assert.AreEqual(resultC.Album, albumC);
        }

        [Test]
        public void SearchCover()
        {
            // Prerequisites
            string filePath = TestPaths.MusicA;
            var temp = ResetFile(filePath);
            temp.Album = "Death";
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);
            client.Galleries.EnableAll();

            CoverSearchResult result = client.SearchCover(filePath);

            // Test
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void SearchCoverWithFilterOnGalleries()
        {
            // Prerequisites
            string filePath = TestPaths.MusicB;
            var temp = ResetFile(filePath);
            temp.Album = "cover";
            temp.Save();

            // Process 1
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);
            client.Galleries.AddLocalGallery(TestPaths.CoverDirectory);
            client.Galleries.AddLocalGallery(TestPaths.CoverDirectoryBis);

            client.Galleries.DisableAll();
            client.Galleries[TestPaths.CoverDirectory].Enable = true;

            CoverSearchResult result = client.SearchCover(filePath);

            // Test 1
            Assert.IsTrue(result.Contains(TestPaths.CoverA));
            Assert.IsFalse(result.Contains(TestPaths.CoverB));

            // Process 2
            client.Galleries.EnableAll();
            result = client.SearchCover(filePath);

            // Test 2
            Assert.IsTrue(result.Contains(TestPaths.CoverA));
            Assert.IsTrue(result.Contains(TestPaths.CoverB));
        }

        static public void AssignCoverOnline<TOnlineGallery>(string filePath, string query)
            where TOnlineGallery : OnlineGallery
        {
            // Prerequisites
            var temp = ResetFile(filePath);
            temp.Album = query;
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            CoverSearchResult result = client.SearchCoverOnline<TOnlineGallery>(filePath);

            client.Files[filePath].Cover = result.First().Cover;
            client.Files[filePath].Save();

            // Test
            Assert.IsTrue(result.Count > 0);
        }

        static public void AssignCoverCached<TOnlineGallery>(string filePath, string query)
            where TOnlineGallery : OnlineGallery
        {
            // Prerequisites
            var temp = ResetFile(filePath);
            temp.Album = query;
            temp.Save();

            var tempClient = new CoverMyOSTClient();
            tempClient.Galleries.ClearAllCache();

            CoverSearchResult tempCover = tempClient.Galleries.SearchCoverOnline<TOnlineGallery>(query);
            tempCover.First().AddToGalleryCache(query);

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            CoverEntry result = client.SearchCoverCached<TOnlineGallery>(filePath);

            client.Files[filePath].Cover = result.Cover;
            client.Files[filePath].Save();
        }

        static public MusicFile LoadFile(string path)
        {
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            return client.Files[path];
        }

        static public MusicFile ResetFile(string path)
        {
            MusicFile file = LoadFile(path);
            file.Album = "";
            file.Cover = null;
            file.Save();

            return file;
        }
    }
}