using System;
using CoverMyOST.Galleries.Base;
using Diese.Modelization;

namespace CoverMyOST.Galleries.Configurators
{
    public abstract class OnlineGalleryConfiguratorBase<TGallery> : IOnlineGalleryConfigurator, IConfigurator<TGallery>
        where TGallery : class, IOnlineGallery
    {
        private TGallery _gallery;

        protected OnlineGalleryConfiguratorBase()
        {
        }

        protected OnlineGalleryConfiguratorBase(TGallery gallery)
        {
            _gallery = gallery;
        }

        public void From(TGallery obj)
        {
            _gallery = obj;
            FromGallery(_gallery);
        }

        protected abstract void FromGallery(TGallery gallery);

        public abstract void Configure(TGallery obj);

        public void Apply()
        {
            if (_gallery == null)
                throw new NullReferenceException();

            Configure(_gallery);
        }
    }
}