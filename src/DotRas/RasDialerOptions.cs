using System.Windows.Forms;

namespace DotRas
{
    /// <summary>
    /// Provides options available to a <see cref="RasDialer"/> for use while dialing a connection.
    /// </summary>
    public class RasDialerOptions
    {
        /// <summary>
        /// Gets or sets the parent window.
        /// </summary>
        /// <remarks>This object is used for dialog box creation and centering when a security DLL has been defined.</remarks>
        public IWin32Window Owner { get; set; }

        /// <summary>
        /// Gets or sets the interface index on top of which the Virtual Private Network (VPN) connection will be dialed.
        /// </summary>
        public int InterfaceIndex { get; set; }
    }
}