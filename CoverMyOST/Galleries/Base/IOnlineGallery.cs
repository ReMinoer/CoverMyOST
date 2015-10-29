using System.Threading;
using System.Threading.Tasks;
using CoverMyOST.Galleries.Configurators;

namespace CoverMyOST.Galleries.Base
{
    public interface IOnlineGallery : ICoversGallery
    {
        bool UseCache { get; set; }
        CoverSearchResult SearchOnline(string query);
        Task<CoverSearchResult> SearchOnlineAsync(string query, CancellationToken ct);
        CoverEntry SearchCached(string query);
        void ClearCache();
        IOnlineGalleryConfigurator GetConfigurator();
    }
}