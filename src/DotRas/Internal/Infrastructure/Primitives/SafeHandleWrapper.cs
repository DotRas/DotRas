using DotRas.Internal.Abstractions.Primitives;
using System;
using System.Runtime.InteropServices;

namespace DotRas.Internal.Infrastructure.Primitives {
    internal class SafeHandleWrapper : ISafeHandleWrapper {
        public SafeHandle UnderlyingHandle { get; }

        public SafeHandleWrapper(SafeHandle handle) {
            UnderlyingHandle = handle ?? throw new ArgumentNullException(nameof(handle));
        }
    }
}
