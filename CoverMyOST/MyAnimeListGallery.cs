using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using MiniMAL;
using MiniMAL.Anime;

namespace CoverMyOST
{
    public class MyAnimeListGallery : ICoversGallery
    {
        private readonly MiniMALClient _miniMal = new MiniMALClient();

        public MyAnimeListGallery()
        {
            _miniMal.Login("TryMiniMAL", "tryminimal");
        }

        public Dictionary<string, Bitmap> Search(string query)
        {
            var result = new Dictionary<string, Bitmap>();

            List<AnimeSearchEntry> search = _miniMal.SearchAnime(query.Split(' '));
            foreach (AnimeSearchEntry entry in search)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(entry.ImageUrl);
                using (var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                using (Stream stream = httpWebReponse.GetResponseStream())
                {
                    if (stream == null)
                        continue;

                    var image = new Bitmap(Image.FromStream(stream));
                    result.Add(entry.Title, image);
                }
            }

            return result;
        }
    }
}