using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialExtensionsBuilder : IRasDialExtensionsBuilder
    {
        private readonly IStructFactory structFactory;

        public RasDialExtensionsBuilder(IStructFactory structFactory)
        {
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
        }

        public RASDIALEXTENSIONS Build(RasDialContext context)
        {
            var rasDialExtensions = structFactory.Create<RASDIALEXTENSIONS>();

            RasDialerOptions options;
            if ((options = context.Options) != null)
            {
                rasDialExtensions.dwfOptions = BuildOptions(options);
            }           

            return rasDialExtensions;
        }

        private static RDEOPT BuildOptions(RasDialerOptions options)
        {
            var builder = new RasDialExtensionsOptionsBuilder();

            builder.AppendFlagIfTrue(options.UsePrefixSuffix, RDEOPT.UsePrefixSuffix);
            builder.AppendFlagIfTrue(options.PausedStates, RDEOPT.PausedStates);
            builder.AppendFlagIfTrue(options.SetModemSpeaker, RDEOPT.SetModemSpeaker);
            builder.AppendFlagIfTrue(options.SetSoftwareCompression, RDEOPT.SetSoftwareCompression);
            builder.AppendFlagIfTrue(options.DisableConnectedUI, RDEOPT.DisableConnectedUI);
            builder.AppendFlagIfTrue(options.DisableReconnectUI, RDEOPT.DisableReconnectUI);
            builder.AppendFlagIfTrue(options.DisableReconnect, RDEOPT.DisableReconnect);
            builder.AppendFlagIfTrue(options.NoUser, RDEOPT.NoUser);
            builder.AppendFlagIfTrue(options.Router, RDEOPT.Router);
            builder.AppendFlagIfTrue(options.CustomDial, RDEOPT.CustomDial);
            builder.AppendFlagIfTrue(options.UseCustomScripting, RDEOPT.UseCustomScripting);

            return builder.Result;
        }
    }
}