using System;
using System.Collections.Generic;
using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;

namespace DotRas.ExtensibleAuthentication
{
    /// <summary>
    /// Represents a credential used by the Extensible Authentication Protocol (EAP). This class cannot be inherited.
    /// </summary>
    public sealed class EapCredential : IEapCredential
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the binary data used during authentication.
        /// </summary>
        public IList<byte> Data { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EapCredential"/> class.
        /// </summary>
        public EapCredential()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EapCredential"/> class.      
        /// </summary>
        /// <param name="userName">The username associated with the credentials.</param>
        /// <param name="data">The binary data used during authentication.</param>
        public EapCredential(string userName, IList<byte> data)
        {
            Username = userName;
            Data = data;
        }

        /// <summary>
        /// Retrieves the credentials for the current user.
        /// </summary>
        /// <param name="phoneBookPath">The full name (including filename)</param>
        /// <param name="entryName"></param>
        /// <returns></returns>
        public static EapCredential ForCurrentUser(string phoneBookPath, string entryName)
        {
            if (string.IsNullOrWhiteSpace(phoneBookPath))
            {
                throw new ArgumentException("The phone book path must be provided.", nameof(phoneBookPath));
            }
            else if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentException("The entry name must be provided.", nameof(entryName));
            }

            return ServiceLocator.Default.GetRequiredService<IRasGetEapCredentials>()
                .ForCurrentUser(phoneBookPath, entryName);
        }
    }
}