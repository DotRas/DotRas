using System;
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
            rasDialParams.dwIfIndex = context.InterfaceIndex;
            
            if (context.Credentials != null)
            {
                rasDialParams.szUserName = context.Credentials.UserName;
                rasDialParams.szPassword = context.Credentials.Password;
                rasDialParams.szDomain = context.Credentials.Domain;
            }

            return rasDialParams;
        }
    }
}