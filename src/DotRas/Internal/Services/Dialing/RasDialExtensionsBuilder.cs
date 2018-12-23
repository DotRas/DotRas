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
            rasDialExtensions.dwfOptions = BuildOptions(context.Options);          

            return rasDialExtensions;
        }

        private static RDEOPT BuildOptions(RasDialerOptions options)
        {
            if (options == null)
            {
                return RDEOPT.None;
            }

            var builder = new RasDialExtensionsOptionsBuilder();
            builder.AppendFlagIfTrue(options.UsePrefixSuffix, RDEOPT.UsePrefixSuffix);

            return builder.Result;
        }
    }
}