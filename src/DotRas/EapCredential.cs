using System;
using DotRas.Internal;
using DotRas.Internal.Interop;
using Microsoft.Win32.SafeHandles;

namespace DotRas
{
    /// <summary>
    /// Represents a credential used by the Extensible Authentication Protocol (EAP). This class cannot be inherited.
    /// </summary>
    public sealed class EapCredential : CriticalHandleZeroOrMinusOneIsInvalid, IEapCredential
    {
        /// <summary>
        /// Gets the handle of the credential.
        /// </summary>
        public IntPtr Handle => handle;

        /// <inheritdoc />
        protected override bool ReleaseHandle()
        {
            ServiceLocator.Default.GetRequiredService<IRasApi32>()
                .RasFreeEapUserIdentity(this);

            return true;
        }
    }
}