using System.Threading;
using System.Threading.Tasks;
using CoverMyOST.Configuration.DataModels;
using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public abstract class CoversGallery : Component<ICoversGallery, ICoversGalleryParent>, ICoversGallery
    {
        public bool Enabled { get; set; }
        public abstract string Description { get; }

        protected CoversGallery()
        {
            Enabled = true;
        }

        public abstract CoverSearchResult Search(string query);
        public abstract Task<CoverSearchResult> SearchAsync(string query, CancellationToken ct);
    }
}