using System.Collections.Generic;
using CoverMyOST.Galleries;

namespace CoverMyOST.Data
{
	public class LocalGalleryData : CoversGalleryData<LocalGallery>
	{
		public override string Name { get; set; }

		public LocalGalleryData() { }
		public LocalGalleryData(LocalGallery obj) { SetData(obj); }

		public override sealed void SetData(LocalGallery obj)
		{
			base.SetData(obj);
			Name = obj.Name;
		}

		public override sealed void SetObject(LocalGallery obj)
		{
			base.SetObject(obj);
			obj.Name = Name;
		}
	}
}