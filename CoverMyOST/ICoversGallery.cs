﻿using System.Collections.Generic;
using System.Drawing;

namespace CoverMyOST
{
    // TODO : Async search
    public interface ICoversGallery
    {
        string Name { get; }
        bool Enable { get; set; }

        Dictionary<string, Bitmap> Search(string query);
    }
}