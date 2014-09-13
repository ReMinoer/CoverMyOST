using NUnit.Framework;

namespace CoverMyOST.Test
{
    class MyAnimeListTest
    {
        [Test]
        public void AssignCoverFromDataBase()
        {
            GalleryTestHelper.AssignCoverFromDataBase<MyAnimeListGallery>("Naruto");
        }
    }
}
