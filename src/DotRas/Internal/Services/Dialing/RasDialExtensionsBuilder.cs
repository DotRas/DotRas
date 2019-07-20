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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var rasDialExtensions = structFactory.Create<RASDIALEXTENSIONS>();

            RasDialerOptions options;
            if ((options = context.Options) != null)
            {
                if (options.Owner != IntPtr.Zero)
                {
                    rasDialExtensions.hwndParent = options.Owner;
                }

                rasDialExtensions.dwfOptions = BuildOptions(options);
            }           

            return rasDialExtensions;
        }

        private static RDEOPT BuildOptions(RasDialerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = new RasDialExtensionsOptionsBuilder();

            return builder.Result;
        }
    }
}