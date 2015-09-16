using System.Linq;
using CoverMyOST.Galleries;
using CoverMyOST.Models.Files;
using CoverMyOST.Test.Content;
using NUnit.Framework;

namespace CoverMyOST.Test
{
    public class LocalGalleryTest
    {
        [Test]
        public void AssignCoverFromGallery()
        {
            // Prerequisites
            string filePath = TestPaths.MusicC;

            // Process
            var loader = new MusicFilesLoader();
            loader.ChangeDirectory(TestPaths.MusicDirectory);

            var localGallery = new LocalGallery("Test", TestPaths.CoverDirectory);

            CoverSearchResult searchResult = localGallery.Search("coverA");

            MusicFile musicFile = ClientTest.ResetFile(filePath);
            musicFile.Cover = searchResult.First().Cover;
            musicFile.Save();

            // Test
            MusicFile resultFile = ClientTest.LoadFile(filePath);
            Assert.AreEqual(searchResult.First().Cover.Size, resultFile.Cover.Size);
        }
    }
}