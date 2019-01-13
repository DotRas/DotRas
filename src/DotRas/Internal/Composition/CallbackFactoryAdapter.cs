using System;
using DotRas.Internal.Abstractions.Composition;

namespace DotRas.Internal.Composition
{
    internal class CallbackFactoryAdapter<T> : ICompositionFactory
    {
        private readonly Func<IServiceProvider, T> factory;

        public CallbackFactoryAdapter(Func<IServiceProvider, T> factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public object CreateObject(IServiceProvider services)
        {
            return factory(services);
        }
    }
}