using System;
using System.IO;
using System.Xml.Serialization;

namespace CoverMyOST
{
	public interface IData<T>
	{
		void SetData(T obj);
		void SetObject(T obj);
	}
}

