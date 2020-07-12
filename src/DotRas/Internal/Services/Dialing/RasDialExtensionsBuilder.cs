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
        private readonly IMarshaller marshaller;

        public RasDialExtensionsBuilder(IStructFactory structFactory, IRasGetEapUserData getEapUserData, IMarshaller marshaller)
        {
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.getEapUserData = getEapUserData ?? throw new ArgumentNullException(nameof(getEapUserData));
            this.marshaller = marshaller ?? throw new ArgumentNullException(nameof(marshaller));
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

                rasDialExtensions.dwfOptions = BuildOptions();
            }

            var eapUserData = getEapUserData.GetEapUserData(IntPtr.Zero, context.EntryName, context.PhoneBookPath);
            if (eapUserData != null)
            {
                var ptr = IntPtr.Zero;

                try
                {
                    ptr = marshaller.ByteArrayToPtr(eapUserData);

                    rasDialExtensions.RasEapInfo.pbEapInfo = ptr;
                    rasDialExtensions.RasEapInfo.dwSizeofEapInfo = eapUserData.Length;
                }
                catch (Exception)
                {
                    marshaller.FreeHGlobalIfNeeded(ptr);
                    throw;
                }
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