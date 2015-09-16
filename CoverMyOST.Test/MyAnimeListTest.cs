﻿using CoverMyOST.Galleries;
using CoverMyOST.Test.Content;
using NUnit.Framework;

namespace CoverMyOST.Test
{
    internal class MyAnimeListTest
    {
        [Test]
        public void AssignCoverOnline()
        {
            ClientTest.AssignCoverOnline(() => new MyAnimeListGallery(), TestPaths.MusicA, "Planetes");
        }

        [Test]
        public void AssignCoverCached()
        {
            ClientTest.AssignCoverCached(() => new MyAnimeListGallery(), TestPaths.MusicA, "Planetes");
        }
    }
}