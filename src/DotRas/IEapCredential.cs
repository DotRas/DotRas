using System;

namespace DotRas
{
    /// <summary>
    /// Identifies a credential used by the Extensible Authentication Protocol (EAP).
    /// </summary>
    public interface IEapCredential
    {
        /// <summary>
        /// Gets the handle of the credential.
        /// </summary>
        IntPtr Handle { get; }
    }
}