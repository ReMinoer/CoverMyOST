namespace CoverMyOST
{
    // TODO : Async search
    public interface ICoversGallery
    {
        string Name { get; }
        bool Enable { get; set; }

        CoverSearchResult Search(string query);
    }
}