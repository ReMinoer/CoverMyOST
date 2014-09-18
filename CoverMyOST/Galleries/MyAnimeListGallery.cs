using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using MiniMAL;
using MiniMAL.Anime;
using System.Threading.Tasks;

namespace CoverMyOST.Galleries
{
	public class MyAnimeListGallery : IOnlineGallery
	{
		public string Name { get { return "MyAnimeList"; } }
		public bool Enable { get; set; }

		private readonly MiniMALClient _miniMal = new MiniMALClient();

        public Dictionary<string, Bitmap> Search(string query)
        {
            if (!_miniMal.IsConnected)
                Login();

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

        private void Login()
        {
            _miniMal.Login("TryMiniMAL", "tryminimal");
        }
    }
}