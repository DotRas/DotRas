using System;

namespace DotRas.Internal.Abstractions.Composition
{
    internal interface ICompositionRegistry
    {
        void RegisterCallback<T>(Func<IServiceProvider, T> callback);
    }
}