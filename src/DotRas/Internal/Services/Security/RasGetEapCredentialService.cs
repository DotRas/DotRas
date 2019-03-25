using System;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;

namespace DotRas.Internal.Services.Security
{
    internal class RasGetEapCredentialService : IRasGetEapCredentials
    {
        private readonly IRasApi32 api;

        public RasGetEapCredentialService(IRasApi32 api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }
    }
}