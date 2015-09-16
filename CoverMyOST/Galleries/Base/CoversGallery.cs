using System.Threading;
using System.Threading.Tasks;
using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public abstract class CoversGallery : Component<ICoversGallery, ICoversGalleryParent>, ICoversGallery
    {
        public bool Enable { get; set; }

        protected CoversGallery()
        {
            Enable = true;
        }

        public abstract CoverSearchResult Search(string query);
        public abstract Task<CoverSearchResult> SearchAsync(string query, CancellationToken ct);
    }
}