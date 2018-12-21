using System;
using System.ComponentModel;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;

namespace DotRas.Internal.Services
{
    internal class AllocateLocallyUniqueIdService : IAllocateLocallyUniqueId
    {
        private readonly IAdvApi32 api;

        public AllocateLocallyUniqueIdService(IAdvApi32 api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Luid AllocateLocallyUniqueId()
        {
            var success = api.AllocateLocallyUniqueId(out var result);
            if (!success)
            {
                throw new Win32Exception();
            }

            return result;
        }
    }
}