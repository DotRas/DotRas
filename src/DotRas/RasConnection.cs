using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;
using DotRas.Win32.SafeHandles;

namespace DotRas
{
    /// <summary>
    /// Represents a remote access connection.
    /// </summary>
    public class RasConnection
    {
        internal RasConnection(RasHandle handle, RasDevice device, string entryName, string phoneBookPath)
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
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnection"/> class.
        /// </summary>
        protected RasConnection()
        {
        }

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

            return Container.Default.GetRequiredService<IRasGetConnectStatus>()
                .GetConnectionStatus(Handle);
        }

        /// <summary>
        /// Terminates the remote access connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
        public virtual void HangUp(CancellationToken cancellationToken)
        {
            GuardHandleMustBeValid();

            Container.Default.GetRequiredService<IRasHangUp>()
                .HangUp(Handle, cancellationToken);
        }

        /// <summary>
        /// Terminates the remote access connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
        public virtual Task HangUpAsync(CancellationToken cancellationToken)
        {
            GuardHandleMustBeValid();

            return Task.Run(() => HangUp(cancellationToken), cancellationToken);
        }

        private void GuardHandleMustBeValid()
        {
            if (Handle.IsClosed || Handle.IsInvalid)
            {
                throw new Exception("The handle is invalid.");
            }
        }
    }
}