using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagLib;

namespace CoverMyOST.Test
{
    [TestFixture]
    public class ClientTest
    {
        private const string TestPath = "Files/test.mp3";

        static private void AssignCoverFromDataBase<TCoversGallery>(string query)
            where TCoversGallery : ICoversGallery, new()
        {
            // Prerequisites
            File temp = File.Create(TestPath);
            temp.Tag.Album = query;
            temp.Save();
            // End Prerequisites

            var client = new CoverMyOSTClient();
            client.LoadFile(TestPath);

            string albumName = client.Files[TestPath].Album;
            ICoversGallery gallery = new TCoversGallery();
            Dictionary<string, Image> cover = gallery.Search(albumName);

            MusicFile file = client.Files[TestPath];
            file.Cover = cover.Values.First();
            file.Save();

            Assert.True(true);
        }

        [Test]
        public void AssignCoverFromMyAnimeList()
        {
            const string animeTitle = "Naruto";
            AssignCoverFromDataBase<MyAnimeListGallery>(animeTitle);
        }
    }
}