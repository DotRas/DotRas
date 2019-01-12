using System;
using DotRas.Internal.Abstractions.DependencyInjection;

namespace DotRas.Internal.DependencyInjection
{
    internal class CallbackFactoryAdapter<T> : ICompositionFactory
    {
        private readonly Func<IServiceProvider, T> factory;

        public CallbackFactoryAdapter(Func<IServiceProvider, T> factory)
        {
            this.factory = factory;
        }

        public object CreateObject(IServiceProvider services)
        {
            return factory(services);
        }
    }
}