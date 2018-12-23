using System.Windows.Forms;

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
        /// Gets or sets the parent window.
        /// </summary>
        /// <remarks>This object is used for dialog box creation and centering when a security DLL has been defined.</remarks>
        public IWin32Window Owner { get; set; }

        /// <summary>
        /// Gets or sets the one-based index of the subentry to dial.
        /// </summary>
        public int SubEntryId { get; set; }

        /// <summary>
        /// Gets or sets the interface index on top of which the Virtual Private Network (VPN) connection will be dialed.
        /// </summary>
        public int InterfaceIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the prefix and suffix that is in the phone book should be used.
        /// </summary>
        public bool UsePrefixSuffix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to accept paused states.
        /// </summary>
        public bool PausedStates { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set the modem speaker.
        /// </summary>
        public bool SetModemSpeaker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable software compression.
        /// </summary>
        public bool SetSoftwareCompression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the connected user interface.
        /// </summary>
        public bool DisableConnectedUI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the reconnect user interface.
        /// </summary>
        public bool DisableReconnectUI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable reconnect.
        /// </summary>
        public bool DisableReconnect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether no user is present.
        /// </summary>
        public bool NoUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connecting device is a router.
        /// </summary>
        public bool Router { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to dial normally instead of calling the custom dial entry point of the custom dialer.
        /// </summary>
        public bool CustomDial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dialer should invoke a custom-scripting DLL after establishing the connection to the server.
        /// </summary>
        public bool UseCustomScripting { get; set; }
    }
}