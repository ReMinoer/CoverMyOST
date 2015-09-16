using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoverMyOST.Galleries.Base;

namespace CoverMyOST.Galleries
{
    public sealed class LocalGallery : CoversGallery
    {
        static private readonly string[] ImageExtensions =
        {
            "*.jpg",
            "*.png",
            "*.gif"
        };

        public string Path { get; set; }

        public LocalGallery(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public override CoverSearchResult Search(string query)
        {
            return SearchAsync(query, CancellationToken.None).Result;
        }

        public override async Task<CoverSearchResult> SearchAsync(string query, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            IEnumerable<string> files =
                ImageExtensions.AsParallel().
                    SelectMany(extensions => Directory.EnumerateFiles(Path, extensions)).
                    Where(file =>
                    {
                        string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                        return filename != null && filename.Contains(query);
                    });

            var result = new CoverSearchResult();
            foreach (string file in files)
            {
                await Task.Yield();
                ct.ThrowIfCancellationRequested();

                result.Add(new CoverEntry(file, new Bitmap(file)));
            }
            return result;
        }
    }
}