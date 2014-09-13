using System.Collections.Generic;
using System.Drawing;

namespace CoverMyOST
{
    public interface ICoversGallery
    {
        Dictionary<string, Image> Search(string query);
    }
}