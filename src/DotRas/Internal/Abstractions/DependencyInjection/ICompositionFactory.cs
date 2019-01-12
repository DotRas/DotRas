using System;

namespace DotRas.Internal.Abstractions.DependencyInjection
{
    internal interface ICompositionFactory
    {
        object CreateObject(IServiceProvider services);
    }
}