using NUnit.Framework;

namespace CoverMyOST.Test
{
    class MyAnimeListTest
    {
        [Test]
        public void AssignCoverFromMyAnimeList()
        {
            const string animeTitle = "Naruto";
            ClientTest.AssignCoverFromDataBase<MyAnimeListGallery>(animeTitle);
        }
    }
}
