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
        private readonly IRasGetCredentials rasGetCredentials;

        public RasDialParamsBuilder(IStructFactory structFactory, IRasGetCredentials rasGetCredentials)
        {
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.rasGetCredentials = rasGetCredentials ?? throw new ArgumentNullException(nameof(rasGetCredentials));
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

            var networkCredentials = GetNetworkCredentials(context);
            if (networkCredentials != null)
            {
                rasDialParams.szUserName = networkCredentials.UserName;
                rasDialParams.szPassword = networkCredentials.Password;
                rasDialParams.szDomain = networkCredentials.Domain;
            }

            return rasDialParams;
        }

        private NetworkCredential GetNetworkCredentials(RasDialContext context)
        {
            if (ShouldUseStoredCredentials(context))
            {
                return rasGetCredentials.GetNetworkCredential(context.EntryName, context.PhoneBookPath);
            }

            return context.Credentials?.UserName;
        }

        private static bool ShouldUseStoredCredentials(RasDialContext context)
        {
            if (context.Options == null)
            {
                return false;
            }

            return context.Options.AllowUseStoredCredentials && 
                   (context.Credentials == null || (context.Credentials != null && context.Credentials.UserName == null));
        }
    }
}