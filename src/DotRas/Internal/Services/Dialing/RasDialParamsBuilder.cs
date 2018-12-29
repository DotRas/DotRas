using System;
using System.Net;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialParamsBuilder : IRasDialParamsBuilder
    {
        private readonly IRasApi32 api;
        private readonly IStructFactory structFactory;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasDialParamsBuilder(IRasApi32 api, IStructFactory structFactory, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public RASDIALPARAMS Build(RasDialContext context)
        {
            var rasDialParams = structFactory.Create<RASDIALPARAMS>();
            rasDialParams.szEntryName = context.EntryName;

            var ret = api.RasGetEntryDialParams(context.PhoneBookPath, ref rasDialParams, out _);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }

            RasDialerOptions options;
            if ((options = context.Options) != null)
            {
                rasDialParams.dwIfIndex = options.InterfaceIndex;
            }

            NetworkCredential credentials;
            if ((credentials = context.Credentials) != null)
            {
                rasDialParams.szUserName = credentials.UserName;
                rasDialParams.szPassword = credentials.Password;
                rasDialParams.szDomain = credentials.Domain;
            }

            return rasDialParams;
        }
    }
}