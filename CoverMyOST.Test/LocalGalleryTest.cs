using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoverMyOST.Test.Content;
using CoverMyOST.Galleries;
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
            ClientTest.ResetFile(filePath);
            var temp = new MusicFile(filePath) {Album = "coverA"};
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);
			client.Galleries.AddLocalGallery(TestPaths.CoverDirectory);

            Dictionary<string, Bitmap> covers = client.SearchCover<LocalGallery>(filePath);

            client.Files[filePath].Cover = covers.Values.First();
            client.Files[filePath].Save();

            // Test
            Assert.IsTrue(covers.Count > 0);
        }
    }
}