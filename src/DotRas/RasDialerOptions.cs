namespace DotRas
{
    /// <summary>
    /// Provides options available to a <see cref="RasDialer"/> for use while dialing a connection.
    /// </summary>
    public class RasDialerOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether stored credentials will be allowed if the credentials have not been provided.
        /// </summary>
        public bool AllowUseStoredCredentials { get; set; }

        /// <summary>
        /// Gets or sets the interface index on top of which the Virtual Private Network (VPN) connection will be dialed.
        /// </summary>
        public int InterfaceIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the prefix and suffix that is in the phone book should be used.
        /// </summary>
        public bool UsePrefixSuffix { get; set; }
    }
}