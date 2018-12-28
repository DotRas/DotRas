using static DotRas.Internal.Interop.Ras;

namespace DotRas
{
    /// <summary>
    /// Defines the different states available for a remote access service (RAS) connection.
    /// </summary>
    /// <remarks>WARNING! Do not write code that depends on the order or occurrence of a particular connection state, because this can vary between platforms.</remarks>
    public enum RasConnectionState
    {
        /// <summary>
        /// The communications port is about to be opened.
        /// </summary>
        OpenPort = 0,

        /// <summary>
        /// The communications port has been opened successfully.
        /// </summary>
        PortOpened,

        /// <summary>
        /// The device is about to be connected.
        /// </summary>
        ConnectDevice,

        /// <summary>
        /// The device has connected successfully.
        /// </summary>
        DeviceConnected,

        /// <summary>
        /// The devices within the device chain have all connected, a physical link has been established.
        /// </summary>
        AllDevicesConnected,

        /// <summary>
        /// The authentication process is starting.
        /// </summary>
        Authenticate,

        /// <summary>
        /// An authentication event has occurred.
        /// </summary>
        AuthNotify,

        /// <summary>
        /// The client has requested another authentication attempt.
        /// </summary>
        AuthRetry,

        /// <summary>
        /// The remote access server has requested a callback number.
        /// </summary>
        AuthCallback,

        /// <summary>
        /// The client has requested to change the password on the account.
        /// </summary>
        AuthChangePassword,

        /// <summary>
        /// The projection phase is starting.
        /// </summary>
        AuthProject,

        /// <summary>
        /// The link speed calculation phase is starting.
        /// </summary>
        AuthLinkSpeed,

        /// <summary>
        /// The authentication request is being acknowledged.
        /// </summary>
        AuthAck,

        /// <summary>
        /// The authentication (after callback) phase is starting.
        /// </summary>
        ReAuthenticate,

        /// <summary>
        /// The client has successfully completed authentication.
        /// </summary>
        Authenticated,

        /// <summary>
        /// The line is about to disconnect in preparation for callback.
        /// </summary>
        PrepareForCallback,

        /// <summary>
        /// The client is delaying in order to give the modem time to reset itself in preparation for callback.
        /// </summary>
        WaitForModemReset,

        /// <summary>
        /// The client is waiting for an incoming call from the remote access server.
        /// </summary>
        WaitForCallback,

        /// <summary>
        /// The projection result information has been made available.
        /// </summary>
        Projected,

        /// <summary>
        /// The user authentication is being started or reattempted.
        /// </summary>
        StartAuthentication,

        /// <summary>
        /// The client has been called back and is about to resume authentication.
        /// </summary>
        CallbackComplete,

        /// <summary>
        /// The client is logging on to the network.
        /// </summary>
        LogOnNetwork,

        /// <summary>
        /// The subentry within a multi-link connection has been connected.
        /// </summary>
        SubEntryConnected,

        /// <summary>
        /// The subentry within a multi-link connection has been disconnected.
        /// </summary>
        SubEntryDisconnected,

        /// <summary>
        /// The client is applying settings.
        /// </summary>
        ApplySettings,

        /// <summary>
        /// The client has entered an interactive state.
        /// </summary>
        Interactive = RASCS_PAUSED,

        /// <summary>
        /// The client is starting to retry authentication.
        /// </summary>
        RetryAuthentication,

        /// <summary>
        /// The client has entered the callback state.
        /// </summary>
        CallbackSetByCaller,

        /// <summary>
        /// The client password has expired.
        /// </summary>
        PasswordExpired,

        /// <summary>
        /// The client has been paused to display a custom authentication user interface.
        /// </summary>
        InvokeEapUI,

        /// <summary>
        /// The client has connected successfully.
        /// </summary>
        Connected = RASCS_DONE,

        /// <summary>
        /// The client has disconnected or failed a connection attempt.
        /// </summary>
        Disconnected,
    }
}