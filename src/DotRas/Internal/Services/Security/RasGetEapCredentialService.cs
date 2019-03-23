using System;
using System.Windows.Forms;
using DotRas.ExtensibleAuthentication;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Security
{
    internal class RasGetEapCredentialService : IRasGetEapCredentials
    {
        private readonly IRasApi32 api;
        private readonly IMarshaller marshal;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasGetEapCredentialService(IRasApi32 api, IMarshaller marshal, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.marshal = marshal ?? throw new ArgumentNullException(nameof(marshal));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public EapCredential ForCurrentUser(string phoneBookPath, string entryName)
        {
            if (string.IsNullOrWhiteSpace(phoneBookPath))
            {
                throw new ArgumentNullException(nameof(phoneBookPath));
            }
            else if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }

            var pRasEapUserIdentity = IntPtr.Zero;

            try
            {
                var ret = api.RasGetEapUserIdentity(phoneBookPath, entryName, RASEAPF.Preview, IntPtr.Zero, out pRasEapUserIdentity);
                if (ret == SUCCESS)
                {
                    return CreateEapCredentialFromPtr(pRasEapUserIdentity);
                }
                else if (ret == ERROR_INVALID_FUNCTION_FOR_ENTRY || ret == ERROR_INTERACTIVE_MODE)
                {
                    return null;
                }
                else
                {
                    throw exceptionPolicy.Create(ret);
                }
            }
            finally
            {
                api.RasFreeEapUserIdentity(pRasEapUserIdentity);
            }
        }

        public EapCredential PromptUserForInformation(string phoneBookPath, string entryName, IWin32Window owner)
        {
            if (string.IsNullOrWhiteSpace(phoneBookPath))
            {
                throw new ArgumentNullException(nameof(phoneBookPath));
            }
            else if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }

            throw new NotImplementedException();
        }

        private EapCredential CreateEapCredentialFromPtr(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptr));
            }

            var rasEapUserIdentity = marshal.PtrToStructure<RASEAPUSERIDENTITY>(ptr);

            var addressOfData = marshal.GetAddressOfPinnedArrayElement(
                rasEapUserIdentity.pbEapInfo, 
                0);

            return new EapCredential(
                rasEapUserIdentity.szUserName,
                marshal.ReadDataFromPtr(addressOfData, rasEapUserIdentity.dwSizeofEapInfo));
        }
    }
}