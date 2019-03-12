using System;
using DotRas.Internal.Abstractions.IoC;

namespace DotRas.Internal.IoC
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