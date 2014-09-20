﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using MiniMAL;
using MiniMAL.Anime;

namespace CoverMyOST.Galleries
{
    public class MyAnimeListGallery : AbstractOnlineGallery
    {
        public override string Name { get { return "MyAnimeList"; } }
        protected override string CacheDirectoryName { get { return "myanimelist"; } }
        private readonly MiniMALClient _miniMal = new MiniMALClient();

        public override CoverSearchResult SearchOnline(string query)
        {
            if (!_miniMal.IsConnected)
                Login();

            var result = new CoverSearchResult();

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
                    result.Add(new CoverSearchEntry(entry.Title, image, this));
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