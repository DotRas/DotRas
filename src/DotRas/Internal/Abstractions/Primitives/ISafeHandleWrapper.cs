using System.Runtime.InteropServices;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface ISafeHandleWrapper
    {
        SafeHandle UnderlyingHandle { get; }
    }
}