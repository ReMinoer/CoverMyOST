using System;
using System.Collections.Generic;
using CoverMyOST.Galleries;

namespace CoverMyOST.Data
{
	public class OnlineGalleryData : CoversGalleryData<OnlineGallery>
	{
		public override string Name { get; set; }
		public bool CacheEnable { get; set; }

		public OnlineGalleryData() { }
		public OnlineGalleryData(OnlineGallery obj) { SetData(obj); }

		public override sealed void SetData(OnlineGallery obj)
		{
			base.SetData(obj);
			Name = obj.Name;
			CacheEnable = obj.CacheEnable;
		}

		public override sealed void SetObject(OnlineGallery obj)
		{
			base.SetObject(obj);
			obj.CacheEnable = CacheEnable;
		}
	}
}