namespace CoverMyOST.Data
{
    public interface IData<in T>
    {
        void SetData(T obj);
        void SetObject(T obj);
    }
}