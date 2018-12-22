using System;
using System.Net;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Security
{
    internal class RasGetCredentialsService : IRasGetCredentials
    {
        private readonly IRasApi32 api;
        private readonly IStructFactory structFactory;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasGetCredentialsService(IRasApi32 api, IStructFactory structFactory, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public NetworkCredential GetNetworkCredential(string entryName, string phoneBookPath)
        {
            if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }

            var rasCredentials = CreateStructure(RASCM.UserName | RASCM.Password | RASCM.Domain);

            var ret = api.RasGetCredentials(phoneBookPath, entryName, ref rasCredentials);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }

            return new NetworkCredential(
                rasCredentials.szUserName,
                rasCredentials.szPassword,
                rasCredentials.szDomain);
        }

        private RASCREDENTIALS CreateStructure(RASCM mask)
        {
            var rasCredentials = structFactory.Create<RASCREDENTIALS>();
            rasCredentials.dwMask = mask;

            return rasCredentials;
        }
    }
}