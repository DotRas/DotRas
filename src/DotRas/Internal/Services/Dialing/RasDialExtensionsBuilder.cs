using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

#pragma warning disable S1854 // False positive

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialExtensionsBuilder : IRasDialExtensionsBuilder
    {
        private readonly IStructFactory structFactory;
        private readonly IRasGetEapUserData getEapUserData;
        
        public RasDialExtensionsBuilder(IStructFactory structFactory, IRasGetEapUserData getEapUserData)
        {
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.getEapUserData = getEapUserData ?? throw new ArgumentNullException(nameof(getEapUserData));
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
                if (options.Owner != null)
                {
                    rasDialExtensions.hwndParent = options.Owner.Handle;
                }

                rasDialExtensions.dwfOptions = BuildOptions();
            }

            if (getEapUserData.TryUnsafeGetEapUserData(IntPtr.Zero, context.EntryName, context.PhoneBookPath, out var eapInfo))
            {
                rasDialExtensions.RasEapInfo = eapInfo;
            }

            return rasDialExtensions;
        }

        private static RDEOPT BuildOptions()
        {
            var builder = new RasDialExtensionsOptionsBuilder();

            return builder.Result;
        }
    }
}