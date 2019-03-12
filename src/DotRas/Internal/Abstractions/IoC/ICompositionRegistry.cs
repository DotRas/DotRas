using System;

namespace DotRas.Internal.Abstractions.IoC
{
    internal interface ICompositionRegistry
    {
        void RegisterCallback<T>(Func<IServiceProvider, T> callback);
    }
}