using System;

namespace DotRas.Internal.Abstractions.Composition
{
    internal interface ICompositionFactory
    {
        object CreateObject(IServiceProvider services);
    }
}