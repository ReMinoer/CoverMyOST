using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoverMyOST.Galleries.Base;
using CoverMyOST.Galleries.Configurators;
using FormPlug.SocketAttributes;
using MiniMAL;
using MiniMAL.Anime;

namespace CoverMyOST.Galleries
{
    // TODO : Create an understandable error when no results
    // TODO : Create a special account for MyAnimeList
    public sealed class MyAnimeListGallery : OnlineGallery
    {
        static private readonly ASCIIEncoding Encoding = new ASCIIEncoding();
        private readonly MiniMALClient _miniMal;
        public string Username { get; set; }
        public byte[] CryptedPassword { get; set; }

        public string Password
        {
            get { return Encoding.GetString(ProtectedData.Unprotect(CryptedPassword, null, DataProtectionScope.CurrentUser)); }
            set { CryptedPassword = ProtectedData.Protect(Encoding.GetBytes(value), null, DataProtectionScope.CurrentUser); }
        }

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

                _miniMal.Login(Username, Password);

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
            finally
            {
                if (_miniMal.IsConnected)
                    _miniMal.Logout();
            }
        }

        public override IOnlineGalleryConfigurator GetConfigurator()
        {
            var configurator = new MyAnimeListGalleryConfigurator();
            configurator.From(this);
            return configurator;
        }

        private sealed class MyAnimeListGalleryConfigurator : OnlineGalleryConfiguratorBase<MyAnimeListGallery>
        {
            [TextSocket]
            public string Username { get; set; }

            [TextSocket(Password = true)]
            public string Password { get; set; }

            public override void Configure(MyAnimeListGallery obj)
            {
                obj.Username = Username;
                obj.Password = Password;
            }

            protected override void FromGallery(MyAnimeListGallery obj)
            {
                Username = obj.Username;
                Password = obj.Password;
            }
        }
    }
}