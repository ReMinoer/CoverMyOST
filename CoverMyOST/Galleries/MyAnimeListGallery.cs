using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoverMyOST.Galleries.Base;
using MiniMAL;
using MiniMAL.Anime;

namespace CoverMyOST.Galleries
{
    // TODO : Fix cancelable request for MyAnimeList
    // TODO : Create an understandable error when no results
    // TODO : Create a special account for MyAnimeList
    public sealed class MyAnimeListGallery : OnlineGallery
    {
        private readonly MiniMALClient _miniMal;

        public override string Description
        {
            get { return "Anime and manga database. Provide one portrait-oriented picture by entry."; }
        }

        public MyAnimeListGallery()
            : base("MyAnimeList", "MyAnimeList")
        {
            _miniMal = new MiniMALClient();
        }

        public override async Task<CoverSearchResult> SearchOnlineAsync(string query, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                if (!_miniMal.IsConnected)
                    Login();

                var result = new CoverSearchResult();

                List<AnimeSearchEntry> search = await _miniMal.SearchAnimeAsync(query, ct);
                ct.ThrowIfCancellationRequested();

                foreach (AnimeSearchEntry entry in search)
                {
                    var request = (HttpWebRequest)WebRequest.Create(entry.ImageUrl);
                    using (ct.Register(() => request.Abort(), false))
                    {
                        try
                        {
                            using (var httpWebReponse = (HttpWebResponse)(await request.GetResponseAsync()))
                            {
                                ct.ThrowIfCancellationRequested();
                                using (Stream stream = httpWebReponse.GetResponseStream())
                                {
                                    if (stream == null)
                                        continue;

                                    var image = new Bitmap(Image.FromStream(stream));
                                    result.Add(new CoverEntry(entry.Title, image, this));
                                }
                            }
                        }
                        catch (WebException e)
                        {
                            if (ct.IsCancellationRequested)
                                throw new OperationCanceledException(e.Message, e, ct);

                            throw;
                        }
                    }
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                return new CoverSearchResult();
            }
        }

        private void Login()
        {
            _miniMal.Login("TryMiniMAL", "tryminimal");
        }
    }
}