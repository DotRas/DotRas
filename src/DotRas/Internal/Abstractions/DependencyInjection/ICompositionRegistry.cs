using System;

namespace DotRas.Internal.Abstractions.DependencyInjection
{
    internal interface ICompositionRegistry
    {
        void RegisterCallback<T>(Func<IServiceProvider, T> callback);
    }
}