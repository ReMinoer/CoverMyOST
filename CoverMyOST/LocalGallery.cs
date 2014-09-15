using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CoverMyOST
{
	public class LocalGallery : ICoversGallery
	{
		public string RootPath { get; private set; }

		public LocalGallery(string path)
		{
			RootPath = path;
		}

		public Dictionary<string, Bitmap> Search(string query)
		{
			IEnumerable<string> files = Directory.EnumerateFiles(RootPath);
			string file = files.First(f => Path.GetFileNameWithoutExtension(f) == query);

			var result = new Dictionary<string, Bitmap>();
			result.Add(file, new Bitmap(Image.FromFile(file)));
			return result;
		}
	}

}

