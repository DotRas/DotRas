using System;
using System.ComponentModel;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.WinBase;

namespace DotRas.Internal.Services.ErrorHandling
{
    internal class Win32FormatMessageService : IWin32FormatMessage
    {
        private readonly IKernel32 api;
        private readonly IMarshaller marshaller;

        public Win32FormatMessageService(IKernel32 api, IMarshaller marshaller)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.marshaller = marshaller ?? throw new ArgumentNullException(nameof(marshaller));
        }

        public string FormatMessage(int errorCode)
        {
            if (errorCode <= 0)
            {
                throw new ArgumentException("The error code must be a positive value.", nameof(errorCode));
            }

            var lpBuffer = IntPtr.Zero;

            try
            {
                var length = api.FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, errorCode, 0, ref lpBuffer, 0, IntPtr.Zero);
                if (ShouldThrowErrorFromLength(length))
                {
                    throw new Win32Exception();
                }

                return marshaller.PtrToUnicodeString(lpBuffer, length);
            }
            finally
            {
                marshaller.FreeHGlobalIfNeeded(lpBuffer);
            }
        }

        private static bool ShouldThrowErrorFromLength(int length)
        {
            return length == 0;
        }
    }
}