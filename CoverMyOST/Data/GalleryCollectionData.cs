using System;
using System.Collections.Generic;
using CoverMyOST.Galleries;
using System.Linq;

namespace CoverMyOST.Data
{
	public class GalleryCollectionData : IData<GalleryCollection>
	{
		public List<LocalGalleryData> LocalDataList { get; set; }
		public List<OnlineGalleryData> OnlineData { get; set; }

		public GalleryCollectionData()
		{
			LocalDataList = new List<LocalGalleryData>();
			OnlineData = new List<OnlineGalleryData>();
		}

		public GalleryCollectionData(GalleryCollection obj) : this()
		{
			SetData(obj);
		}

		public void SetData(GalleryCollection obj)
		{
			LocalDataList.Clear();
			foreach (LocalGallery local in obj.Local)
				LocalDataList.Add(new LocalGalleryData(local));

			OnlineData.Clear();
			foreach (ICoversGallery online in obj.List.Where(gallery => gallery is OnlineGallery))
				OnlineData.Add(new OnlineGalleryData((online as OnlineGallery)));
		}

		public void SetObject(GalleryCollection obj)
		{
			obj.ClearLocal();
			foreach (LocalGalleryData localData in LocalDataList)
			{
				var local = new LocalGallery(localData.Name);
				localData.SetObject(local);
				obj.AddLocal(local);
			}

			foreach (OnlineGalleryData onlineData in OnlineData)
				onlineData.SetObject(obj[onlineData.Name] as OnlineGallery);
		}
	}
}