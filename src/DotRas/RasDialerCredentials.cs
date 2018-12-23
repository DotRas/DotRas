using System.Net;

namespace DotRas
{
    /// <summary>
    /// Describes the credentials available while dialing a connection.
    /// </summary>
    public class RasDialerCredentials
    {
        /// <summary>
        /// Gets or sets the user name and password to use during authentication.
        /// </summary>
        public NetworkCredential UserName { get; set; }
    }
}