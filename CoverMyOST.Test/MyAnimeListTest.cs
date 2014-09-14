using CoverMyOST.Galleries;
using CoverMyOST.Test.Content;
using NUnit.Framework;

namespace CoverMyOST.Test
{
    internal class MyAnimeListTest
    {
        [Test]
        public void SearchCoverInGallery()
        {
            ClientTest.SearchCoverInGallery<MyAnimeListGallery>(TestPaths.MusicA, "Naruto");
        }
    }
}