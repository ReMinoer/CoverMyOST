using CoverMyOST.Test.Helpers;
using NUnit.Framework;

namespace CoverMyOST.Test
{
    internal class MyAnimeListTest
    {
        [Test]
        public void AssignCoverFromDataBase()
        {
            GalleryTestHelper.AssignCoverFromDataBase<MyAnimeListGallery>(TestPaths.MusicA, "Naruto");
        }
    }
}