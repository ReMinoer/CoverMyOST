using System.Threading;
using System.Threading.Tasks;
using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public interface ICoversGallery : IComponent<ICoversGallery, ICoversGalleryParent>
    {
        bool Enable { get; set; }
        CoverSearchResult Search(string query);
        Task<CoverSearchResult> SearchAsync(string query, CancellationToken ct);
    }
}