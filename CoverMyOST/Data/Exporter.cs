using System.IO;
using System.Xml.Serialization;

namespace CoverMyOST.Data
{
	public class Exporter<T>
		where T : new()
	{
		public T LoadXML(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			var obj = (T)serializer.Deserialize(streamReader);
			streamReader.Close();
			return obj;
		}

		public void SaveXML(T obj, string path)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			serializer.Serialize(streamWriter, obj);
			streamWriter.Close();
		}
	}

	public class Exporter<T,TData>
		where TData : IData<T>, new()
	{
		public void LoadXML(T obj, string path)
		{
			StreamReader streamReader = new StreamReader(path);
			XmlSerializer serializer = new XmlSerializer(typeof(TData));

			var data = (TData)serializer.Deserialize(streamReader);
			data.SetObject(obj);

			streamReader.Close();
		}

		public void SaveXML(T obj, string path)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			XmlSerializer serializer = new XmlSerializer(typeof(TData));

			var data = new TData();
			data.SetData(obj);
			serializer.Serialize(streamWriter, data);

			streamWriter.Close();
		}
	}
}