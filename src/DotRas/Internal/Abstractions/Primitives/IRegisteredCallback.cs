using System;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface IRegisteredCallback : IDisposable
    {
        ISafeHandleWrapper Handle { get; }
    }
}