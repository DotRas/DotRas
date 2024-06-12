using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using System;
using System.ComponentModel;

namespace DotRas.Internal.Services {
    internal class AllocateLocallyUniqueIdService : IAllocateLocallyUniqueId {
        private readonly IAdvApi32 api;

        public AllocateLocallyUniqueIdService(IAdvApi32 api) {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Luid AllocateLocallyUniqueId() {
            var success = api.AllocateLocallyUniqueId(out var result);
            return !success ? throw new Win32Exception() : result;
        }
    }
}
