using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagLib;

namespace CoverMyOST.Test
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
            client.AddFile(filePath);

            string albumName = client.Files[filePath].Album;
            ICoversGallery gallery = new TCoversGallery();
            Dictionary<string, Image> cover = gallery.Search(albumName);

            MusicFile file = client.Files[filePath];
            file.Cover = cover.Values.First();
            file.Save();
        }
    }
}