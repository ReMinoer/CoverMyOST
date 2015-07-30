using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MiniMAL;
using MiniMAL.Anime;

namespace CoverMyOST.Galleries
{
    // TODO : Fix cancelable request for MyAnimeList
    // TODO : Create an understandable error when no results
    // TODO : Create a special account for MyAnimeList
    public class MyAnimeListGallery : OnlineGallery
    {
        private readonly MiniMALClient _miniMal = new MiniMALClient();

        public override string Name
        {
            get { return "MyAnimeList"; }
        }

        protected override string CacheDirectoryName
        {
            get { return "myanimelist"; }
        }

        public override async Task<CoverSearchResult> SearchOnlineAsync(string query)
        {
            if (!_miniMal.IsConnected)
                Login();

            var result = new CoverSearchResult();

            List<AnimeSearchEntry> search = await _miniMal.SearchAnimeAsync(query.Split(' '));
            foreach (AnimeSearchEntry entry in search)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(entry.ImageUrl);
                using (var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (Stream stream = httpWebReponse.GetResponseStream())
                    {
                        if (stream == null)
                            continue;

                        var image = new Bitmap(Image.FromStream(stream));
                        result.Add(new CoverEntry(entry.Title, image, this));
                    }
            }

            return result;
        }

        public override void CancelSearch()
        {
            _miniMal.CancelCurrentRequest();
        }

        private void Login()
        {
            _miniMal.Login("TryMiniMAL", "tryminimal");
        }
    }
}