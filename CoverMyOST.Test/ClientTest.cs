using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using MiniMAL;
using MiniMAL.Anime;
using NUnit.Framework;
using TagLib.Riff;
using File = TagLib.File;

namespace CoverMyOST.Test
{
    [TestFixture]
    public class ClientTest
    {
        private const string Mp3FilePath = "Files/test.mp3";

        private static IClient GetClient()
        {
            return new CoverMyOSTClient();
        }

        [Test]
        public void CoverFromAlbumName()
        {
            const string animeTitle = "Clannad";

            File file = new TagLib.Mpeg.File(Path.Combine(Environment.CurrentDirectory, Mp3FilePath));
            file.Tag.Album = animeTitle;

            IClient client = GetClient();
            client.CoverFromAlbumName(file);

            Assert.True(true);
        }
    }
}
