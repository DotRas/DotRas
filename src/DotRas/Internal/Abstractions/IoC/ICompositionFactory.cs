using System;

namespace DotRas.Internal.Abstractions.IoC
{
    internal interface ICompositionFactory
    {
        object CreateObject(IServiceProvider services);
    }
}