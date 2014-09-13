using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagLib;

namespace CoverMyOST.Test
{
    static public class GalleryTestHelper
    {
        static public void AssignCoverFromDataBase<TCoversGallery>(string query)
            where TCoversGallery : ICoversGallery, new()
        {
            // Prerequisites
            File temp = File.Create(ClientTest.TestPathB);
            temp.Tag.Album = query;
            temp.Save();

            // Process
            var client = new CoverMyOSTClient();
            client.AddFile(ClientTest.TestPathB);

            string albumName = client.Files[ClientTest.TestPathB].Album;
            ICoversGallery gallery = new TCoversGallery();
            Dictionary<string, Image> cover = gallery.Search(albumName);

            MusicFile file = client.Files[ClientTest.TestPathB];
            file.Cover = cover.Values.First();
            file.Save();
        }
    }
}
