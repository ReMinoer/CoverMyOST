using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Diese.Composition;

namespace CoverMyOST.Galleries.Base
{
    public class CoversGalleryComposite<T> : Composite<ICoversGallery, ICoversGalleryParent, T>, ICoversGalleryComposite<T>
        where T : class, ICoversGallery
    {
        public bool Enabled { get; set; }

        public virtual string Description
        {
            get { return string.Join(", ", this.Select(x => x.Description)); }
        }

        public CoverSearchResult Search(string query)
        {
            var result = new CoverSearchResult();

            foreach (T coverGallery in this)
                result.AddRange(coverGallery.Search(query));

            return result;
        }

        public async Task<CoverSearchResult> SearchAsync(string query, CancellationToken ct)
        {
            var result = new CoverSearchResult();

            foreach (T coverGallery in this)
            {
                ct.ThrowIfCancellationRequested();
                result.AddRange(coverGallery.Search(query));
                await Task.Yield();
            }

            return result;
        }
    }
}