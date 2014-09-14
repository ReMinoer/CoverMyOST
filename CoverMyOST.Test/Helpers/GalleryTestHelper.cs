using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using File = TagLib.File;

namespace CoverMyOST.Test.Helpers
{
    static internal class GalleryTestHelper
    {
        static public void AssignCoverFromDataBase<TCoversGallery>(string filePath, string query)
            where TCoversGallery : ICoversGallery, new()
        {
            // Prerequisites
            File temp = File.Create(filePath);
            temp.Tag.Album = query;
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.ChangeDirectory(TestPaths.MusicDirectory);

            string albumName = client.Files[filePath].Album;
            ICoversGallery gallery = new TCoversGallery();
            Dictionary<string, Bitmap> covers = gallery.Search(albumName);

            Bitmap cover = covers.Values.First();
            MusicFile file = client.Files[filePath];
            file.Cover = cover;
            file.Save();

            // Test
            var result = new MusicFile(filePath).Cover;
            Assert.AreEqual(result.Size, cover.Size);
        }
    }
}