using CoverMyOST.Galleries;
using CoverMyOST.Test.Content;
using NUnit.Framework;

namespace CoverMyOST.Test
{
    internal class MyAnimeListTest
    {
        [Test]
		public void AssignCoverFromOnlineGallery()
        {
			ClientTest.AssignCoverFromOnlineGallery<MyAnimeListGallery>(TestPaths.MusicA, "Planetes");
        }
    }
}