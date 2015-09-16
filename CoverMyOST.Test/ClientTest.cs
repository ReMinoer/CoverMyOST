using System;
using System.Drawing;
using System.Linq;
using CoverMyOST.Galleries.Base;
using CoverMyOST.Models.Files;
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
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            // Test
            Assert.AreEqual(loader.WorkingDirectory, TestPaths.MusicDirectory);
            Assert.IsTrue(loader.Files.ContainsKey(TestPaths.MusicA));
            Assert.IsTrue(loader.Files.ContainsKey(TestPaths.MusicB));
            Assert.IsTrue(loader.Files.ContainsKey(TestPaths.MusicC));
        }

        [Test]
        public void FilterFiles()
        {
            // Prerequisites
            MusicFile temp = ResetFile(TestPaths.MusicA);
            MusicFile temp2 = ResetFile(TestPaths.MusicB);
            ResetFile(TestPaths.MusicC);

            temp.Album = "Death";
            temp.Cover = new Bitmap(Image.FromFile(TestPaths.CoverA));
            temp.Save();

            temp2.Album = "Angel";
            temp2.Save();

            // Process 1
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            var filter = new MusicFilesFilter(loader)
            {
                Filter = MusicFileFilter.NoAlbum
            };

            // Test 1
            Assert.IsFalse(filter.Files.ContainsKey(TestPaths.MusicA));
            Assert.IsFalse(filter.Files.ContainsKey(TestPaths.MusicB));
            Assert.IsTrue(filter.Files.ContainsKey(TestPaths.MusicC));

            // Process 2
            filter.Filter = MusicFileFilter.NoCover;

            // Test 2
            Assert.IsFalse(filter.Files.ContainsKey(TestPaths.MusicA));
            Assert.IsTrue(filter.Files.ContainsKey(TestPaths.MusicB));
            Assert.IsTrue(filter.Files.ContainsKey(TestPaths.MusicC));
        }

        [Test]
        public void EditAlbumTag()
        {
            // Prerequisites
            string filePath = TestPaths.MusicA;
            ResetFile(filePath);

            // Process
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            const string name = "Insert an album name here";
            loader.Files[filePath].Album = name;
            loader.Files[filePath].Save();

            // Test
            MusicFile result = LoadFile(filePath);
            Assert.AreEqual(result.Album, name);
        }

        [Test]
        public void EditCover()
        {
            // Prerequisites
            string filePath = TestPaths.MusicB;
            ResetFile(filePath);

            // Process
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            var cover = new Bitmap(Image.FromFile(TestPaths.CoverA));
            loader.Files[filePath].Cover = cover;
            loader.Files[filePath].Save();

            // Test
            MusicFile result = LoadFile(filePath);
            Assert.AreEqual(result.Cover.Size, cover.Size);
        }

        static public void AssignCoverOnline(Func<IOnlineGallery> galleryFactory, string filePath, string query)
        {
            // Prerequisites
            MusicFile temp = ResetFile(filePath);

            // Process
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            IOnlineGallery gallery = galleryFactory();
            CoverSearchResult searchResult = gallery.SearchOnline(query);

            MusicFile file = LoadFile(filePath);
            file.Cover = searchResult.First().Cover;
            file.Save();

            // Test
            MusicFile resultFile = LoadFile(filePath);
            Assert.AreEqual(searchResult.First().Cover.Size, resultFile.Cover.Size);
        }

        static public void AssignCoverCached(Func<IOnlineGallery> galleryFactory, string filePath, string query)
        {
            // Prerequisites
            MusicFile temp = ResetFile(filePath);
            temp.Album = query;
            temp.Save();

            IOnlineGallery gallery = galleryFactory();
            gallery.ClearCache();

            CoverSearchResult tempCover = gallery.SearchOnline(query);
            tempCover.First().AddToGalleryCache(query);

            // Process
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            CoverEntry coverEntry = gallery.SearchCached(query);

            MusicFile file = LoadFile(filePath);
            file.Cover = coverEntry.Cover;
            file.Save();

            // Test
            MusicFile resultFile = LoadFile(filePath);
            Assert.AreEqual(coverEntry.Cover.Size, resultFile.Cover.Size);
        }

        static public MusicFile LoadFile(string path)
        {
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            return loader.Files[path];
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