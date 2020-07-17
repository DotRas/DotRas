using System;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

#pragma warning disable S1854 // Unused assignments should be removed

namespace DotRas.Internal.Services.Security
{
    internal class RasGetEapUserDataService : IRasGetEapUserData
    {
        private readonly IRasApi32 api;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IMarshaller marshaller;

        public RasGetEapUserDataService(IRasApi32 api, IExceptionPolicy exceptionPolicy, IMarshaller marshaller)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.marshaller = marshaller ?? throw new ArgumentNullException(nameof(marshaller));
        }

        public bool TryUnsafeGetEapUserData(IntPtr impersonationToken, string entryName, string phoneBookPath, out RASEAPINFO eapInfo)
        {
            if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }

            var pbEapData = IntPtr.Zero;
            var pdwSizeofEapData = 0;
            
            try
            {
                bool retry;

                do
                {
                    retry = false;
                    pbEapData = marshaller.AllocHGlobal(pdwSizeofEapData);

                    var ret = api.RasGetEapUserData(impersonationToken, phoneBookPath, entryName, pbEapData, ref pdwSizeofEapData);
                    if (ret == ERROR_BUFFER_TOO_SMALL)
                    {
                        marshaller.FreeHGlobalIfNeeded(pbEapData);
                        retry = true;
                    }
                    else if (ret != SUCCESS)
                    {
                        throw exceptionPolicy.Create(ret);
                    }
                } while (retry);
            }
            catch (Exception)
            {
                marshaller.FreeHGlobalIfNeeded(pbEapData);
                throw;
            }

            if (pbEapData != IntPtr.Zero && pdwSizeofEapData > 0)
            {
                eapInfo = new RASEAPINFO
                {
                    pbEapInfo = pbEapData,
                    dwSizeofEapInfo = pdwSizeofEapData
                };

                return true;
            }

            eapInfo = new RASEAPINFO
            {
                dwSizeofEapInfo = 0,
                pbEapInfo = IntPtr.Zero
            };

            return false;
        }
    }
}