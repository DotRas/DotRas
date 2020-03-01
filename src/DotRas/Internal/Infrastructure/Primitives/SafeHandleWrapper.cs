using System;
using System.Runtime.InteropServices;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Infrastructure.Primitives
{
    internal class SafeHandleWrapper : ISafeHandleWrapper
    {
        public SafeHandle UnderlyingHandle { get; }

        public SafeHandleWrapper(SafeHandle handle)
        {
            UnderlyingHandle = handle ?? throw new ArgumentNullException(nameof(handle));
        }
    }
}