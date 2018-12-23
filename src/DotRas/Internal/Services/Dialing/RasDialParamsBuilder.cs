using System;
using System.Net;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialParamsBuilder : IRasDialParamsBuilder
    {
        private readonly IStructFactory structFactory;

        public RasDialParamsBuilder(IStructFactory structFactory)
        {
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
        }

        public RASDIALPARAMS Build(RasDialContext context)
        {
            var rasDialParams = structFactory.Create<RASDIALPARAMS>();
            rasDialParams.szEntryName = context.EntryName;

            RasDialerOptions options;
            if ((options = context.Options) != null)
            {
                rasDialParams.dwIfIndex = options.InterfaceIndex;
                rasDialParams.dwSubEntry = options.SubEntryId;
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