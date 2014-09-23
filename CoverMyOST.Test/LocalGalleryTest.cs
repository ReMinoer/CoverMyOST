using System.Linq;
using CoverMyOST.Galleries;
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
            var temp = ClientTest.ResetFile(filePath);
            temp.Album = "coverA";
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);
            client.Galleries.AddLocal(TestPaths.CoverDirectory);

            CoverSearchResult searchResult = client.SearchCover<LocalGallery>(filePath);

            client.Files[filePath].Cover = searchResult.First().Cover;
            client.Files[filePath].Save();

            // Test
            var resultFile = ClientTest.LoadFile(filePath);
            Assert.AreEqual(searchResult.First().Cover.Size, resultFile.Cover.Size);
        }
    }
}