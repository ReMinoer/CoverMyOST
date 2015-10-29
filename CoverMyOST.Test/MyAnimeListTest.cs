using CoverMyOST.Galleries;
using CoverMyOST.Galleries.Base;
using CoverMyOST.Test.Content;
using NUnit.Framework;

namespace CoverMyOST.Test
{
    internal class MyAnimeListTest
    {
        [Test]
        public void AssignCoverOnline()
        {
            ClientTest.AssignCoverOnline(GalleryFactory, TestPaths.MusicA, "Planetes");
        }

        [Test]
        public void AssignCoverCached()
        {
            ClientTest.AssignCoverCached(GalleryFactory, TestPaths.MusicA, "Planetes");
        }

        private IOnlineGallery GalleryFactory()
        {
            return new MyAnimeListGallery
            {
                Username = "TryMiniMAL",
                Password = "tryminimal"
            };
        }
    }
}