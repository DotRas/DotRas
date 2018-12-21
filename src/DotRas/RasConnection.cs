using System;
using System.Collections.Generic;
using System.Threading;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;

namespace DotRas
{
    /// <summary>
    /// Represents a remote access connection.
    /// </summary>
    public class RasConnection
    {
        #region Fields and Properties

        private readonly IRasGetConnectStatus getConnectStatusService;
        private readonly IRasHangUp hangUpService;

        /// <summary>
        /// Gets the handle of the connection.
        /// </summary>
        public virtual RasHandle Handle { get; }

        /// <summary>
        /// Gets the device through which the connection has been established.
        /// </summary>
        public virtual RasDevice Device { get; }

        /// <summary>
        /// Gets the name of the phone book entry used to establish the remote access connection.
        /// </summary>
        public virtual string EntryName { get; }

        /// <summary>
        /// Gets the full path and filename to the phone book (PBK) containing the entry for this connection.
        /// </summary>
        public virtual string PhoneBookPath { get; }

        /// <summary>
        /// Gets the one-based sub-entry index of the connected link in a multi-link connection.
        /// </summary>
        public virtual int SubEntryId { get; }

        /// <summary>
        /// Gets the <see cref="Guid"/> that represents the phone book entry.
        /// </summary>
        public virtual Guid EntryId { get; }

        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public virtual RasConnectionOptions Options { get; }

        /// <summary>
        /// Gets the <see cref="Luid"/> that represents the logon session in which the connection was established.
        /// </summary>
        public virtual Luid SessionId { get; }

        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public virtual Guid CorrelationId { get; }

        #endregion

        internal RasConnection(RasHandle handle, RasDevice device, string entryName, string phoneBookPath, int subEntryId, Guid entryId, RasConnectionOptions options, Luid sessionId, Guid correlationId, IRasGetConnectStatus getConnectStatusService, IRasHangUp hangUpService)
        {
            if (handle == null)
            {
                throw new ArgumentNullException(nameof(handle));
            }
            else if (handle.IsClosed)
            {
                throw new ArgumentException("The handle provided must not be closed.", nameof(handle));
            }
            else if (handle.IsInvalid)
            {
                throw new ArgumentException("The handle is invalid.", nameof(handle));
            }
            else if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }
            else if (string.IsNullOrWhiteSpace(phoneBookPath))
            {
                throw new ArgumentNullException(nameof(phoneBookPath));
            }

            EntryName = entryName;
            PhoneBookPath = phoneBookPath;
            Handle = handle;
            Device = device ?? throw new ArgumentNullException(nameof(device));
            SubEntryId = subEntryId;
            EntryId = entryId;
            Options = options ?? throw new ArgumentNullException(nameof(options));
            SessionId = sessionId;
            CorrelationId = correlationId;

            this.getConnectStatusService = getConnectStatusService ?? throw new ArgumentNullException(nameof(getConnectStatusService));
            this.hangUpService = hangUpService ?? throw new ArgumentNullException(nameof(hangUpService));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnection"/> class.
        /// </summary>
        protected RasConnection()
        {
        }

        /// <summary>
        /// Enumerates the connections.
        /// </summary>
        /// <returns>An enumerable used to iterate through the connections.</returns>
        public static IEnumerable<RasConnection> EnumerateConnections()
        {
            return Container.Default.GetRequiredService<IRasEnumConnections>()
                .EnumerateConnections();
        }

        /// <summary>
        /// Retrieves the connection status.
        /// </summary>
        public virtual RasConnectionStatus GetConnectionStatus()
        {
            GuardHandleMustBeValid();

            return getConnectStatusService.GetConnectionStatus(Handle);
        }

        /// <summary>
        /// Terminates the remote access connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
        public virtual void HangUp(CancellationToken cancellationToken)
        {
            GuardHandleMustBeValid();

            hangUpService.HangUp(Handle, cancellationToken);
        }

        private void GuardHandleMustBeValid()
        {
            if (Handle.IsClosed || Handle.IsInvalid)
            {
                throw new InvalidHandleException("The handle is invalid.");
            }
        }
    }
}