using System.Collections.Generic;

namespace CoverMyOST.Data
{
	public abstract class CoversGalleryData<TCoversGallery> : IData<TCoversGallery>
		where TCoversGallery : ICoversGallery
	{
		public abstract string Name { get; set; }
		public bool Enable { get; set; }

		public virtual void SetData(TCoversGallery obj)
		{
			Enable = obj.Enable;
		}

		public virtual void SetObject(TCoversGallery obj)
		{
			obj.Enable = Enable;
		}
	}
}