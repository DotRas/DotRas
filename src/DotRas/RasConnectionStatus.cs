namespace DotRas
{
    /// <summary>
    /// Represents the current status of a remote access connection.
    /// </summary>
    public class RasConnectionStatus
    {
        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        public virtual RasConnectionState ConnectionState { get; }

        /// <summary>
        /// Gets the device through which the connection has been established.
        /// </summary>
        public virtual Device Device { get; }

        /// <summary>
        /// Gets the phone number dialed for this specific connection.
        /// </summary>
        public virtual string PhoneNumber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionState"/> class.
        /// </summary>
        /// <param name="connectionState">The state of the connection.</param>
        /// <param name="device">The device through which the connection has been established.</param>
        /// <param name="phoneNumber">The phone number dialed for this specific connection.</param>
        public RasConnectionStatus(RasConnectionState connectionState, Device device, string phoneNumber)
        {
            ConnectionState = connectionState;
            Device = device;
            PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionState"/> class.
        /// </summary>
        protected RasConnectionStatus()
        {
        }
    }
}