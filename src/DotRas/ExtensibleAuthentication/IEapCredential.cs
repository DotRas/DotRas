using System.Collections.Generic;

namespace DotRas.ExtensibleAuthentication
{
    /// <summary>
    /// Identifies a credential used by the Extensible Authentication Protocol (EAP).
    /// </summary>
    public interface IEapCredential
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the binary data used during authentication.
        /// </summary>
        IList<byte> Data { get; set; }
    }
}