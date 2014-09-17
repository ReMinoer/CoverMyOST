using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CoverMyOST.Galleries
{
	public class LocalGallery : ICoversGallery
	{
		public string Name { get; private set; }
		public bool Enable { get; set; }

        private readonly string[] _imagePatterns = {"*.jpg", "*.png", "*.gif"};

        public LocalGallery(string path)
        {
			Name = path;
        }

        public Dictionary<string, Bitmap> Search(string query)
        {
            IEnumerable<string> files =
                _imagePatterns.AsParallel().
							   SelectMany(imagePattern => Directory.EnumerateFiles(Name, imagePattern)).
                               Where(file =>
                               {
                                   string filename = Path.GetFileNameWithoutExtension(file);
                                   return filename != null && filename.Contains(query);
                               });

            var result = new Dictionary<string, Bitmap>();
            foreach (string file in files)
                result.Add(file, new Bitmap(Image.FromFile(file)));
            return result;
        }
    }
}