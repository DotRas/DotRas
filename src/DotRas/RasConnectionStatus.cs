using System;
using System.Net;

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
        /// Gets the error code.
        /// </summary>
        public virtual int? ErrorCode { get; }

        /// <summary>
        /// Gets the device through which the connection has been established.
        /// </summary>
        public virtual RasDevice Device { get; }

        /// <summary>
        /// Gets the phone number dialed for this specific connection.
        /// </summary>
        public virtual string PhoneNumber { get; }

        /// <summary>
        /// Gets the local client endpoint information of a virtual private network (VPN) tunnel.
        /// </summary>
        public virtual IPAddress LocalEndPoint { get; }

        /// <summary>
        /// Gets the remote server endpoint information of a virtual private network (VPN) tunnel.
        /// </summary>
        public virtual IPAddress RemoteEndPoint { get; }

        /// <summary>
        /// Gets the state of an Internet Key Exchange version 2 (IKEv2) virtual private network (VPN) tunnel.
        /// </summary>
        public virtual RasConnectionSubState ConnectionSubState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionState"/> class.
        /// </summary>
        /// <param name="connectionState">The state of the connection.</param>
        /// <param name="errorCode">Optional. The error code (if any occurred).</param>
        /// <param name="device">The device through which the connection has been established.</param>
        /// <param name="phoneNumber">The phone number dialed for this specific connection.</param>
        /// <param name="localEndpoint">Optional. The local client endpoint information of a virtual private network (VPN) tunnel.</param>
        /// <param name="remoteEndpoint">Optional. The remote client endpoint information of a virtual private network (VPN) tunnel.</param>
        /// <param name="connectionSubState">The state of an Internet Key Exchange version2 (IKEv2) virtual private network (VPN) tunnel.</param>
        public RasConnectionStatus(RasConnectionState connectionState, int? errorCode, RasDevice device, string phoneNumber, IPAddress localEndpoint, IPAddress remoteEndpoint, RasConnectionSubState connectionSubState)
        {
            ConnectionState = connectionState;
            ErrorCode = errorCode;
            Device = device ?? throw new ArgumentNullException(nameof(device));
            PhoneNumber = phoneNumber;
            LocalEndPoint = localEndpoint;
            RemoteEndPoint = remoteEndpoint;
            ConnectionSubState = connectionSubState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionState"/> class.
        /// </summary>
        protected RasConnectionStatus()
        {
        }
    }
}