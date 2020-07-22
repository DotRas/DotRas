using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;

namespace DotRas
{
    /// <summary>
    /// Provides a mechanism to establish a connection to a remote network.
    /// </summary>
    public class RasDialer : RasComponentBase
    {
        #region Fields and Properties

        private readonly IRasDial api;

        /// <summary>
        /// Gets a value indicating whether this instance is currently dialing a connection.
        /// </summary>
        public bool IsBusy => api.IsBusy;

        /// <summary>
        /// Gets or sets the name of the entry within the phone book.
        /// </summary>
        public string EntryName { get; set; }

        /// <summary>
        /// Gets the full path (including filename) of the phone book containing the entry.
        /// </summary>
        public string PhoneBookPath { get; set; }

        /// <summary>
        /// Gets or sets the credentials to use while dialing the connection.
        /// </summary>
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Gets the options configurable for a dial attempt.
        /// </summary>
        public RasDialerOptions Options { get; } = new RasDialerOptions();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the state changes while connecting to a remote network.
        /// </summary>
        /// <remarks>
        /// Please note, this event is only raised while a connection is being established. It will not be raised if
        /// an active connection has been disconnected outside of an attempt to connect.
        /// </remarks>
        public event EventHandler<StateChangedEventArgs> StateChanged;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RasDialer"/> class.
        /// </summary>
        public RasDialer() 
            : this(ServiceLocator.Default.GetRequiredService<IRasDial>())
        {
        }

        internal RasDialer(IRasDial api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        /// <summary>
        /// Connects to the remote network.
        /// </summary>
        /// <returns>The connection instance.</returns>
        /// <exception cref="EapException">Thrown when an error occurs while authenticating the user credentials when using Extensible Authentication Protocol (EAP).</exception>
        /// <exception cref="IPSecException">Thrown when an error occurs while dialing the connection when using IPSec.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasException">Thrown when an error occurs while dialing the connection.</exception>
        /// <exception cref="Win32Exception">Thrown when an error occurs while dialing the connection.</exception>
        public RasConnection Connect()
        {
            return Connect(CancellationToken.None);
        }

        /// <summary>
        /// Connects to the remote network.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests while dialing the connection.</param>
        /// <returns>The connection instance.</returns>
        /// <exception cref="EapException">Thrown when an error occurs while authenticating the user credentials when using Extensible Authentication Protocol (EAP).</exception>
        /// <exception cref="IPSecException">Thrown when an error occurs while dialing the connection when using IPSec.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasException">Thrown when an error occurs while dialing the connection.</exception>
        /// <exception cref="Win32Exception">Thrown when an error occurs while dialing the connection.</exception>
        public RasConnection Connect(CancellationToken cancellationToken)
        {
            return ConnectAsync(cancellationToken).GetResultSynchronously();
        }

        /// <summary>
        /// Connects to the remote network asynchronously.
        /// </summary>
        /// <returns>The connection instance.</returns>
        /// <exception cref="EapException">Thrown when an error occurs while authenticating the user credentials when using Extensible Authentication Protocol (EAP).</exception>
        /// <exception cref="IPSecException">Thrown when an error occurs while dialing the connection when using IPSec.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasException">Thrown when an error occurs while dialing the connection.</exception>
        /// <exception cref="Win32Exception">Thrown when an error occurs while dialing the connection.</exception>
        public Task<RasConnection> ConnectAsync()
        {
            return ConnectAsync(CancellationToken.None);
        }

        /// <summary>
        /// Connects to the remote network asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests while dialing the connection.</param>
        /// <returns>The connection instance.</returns>
        /// <exception cref="EapException">Thrown when an error occurs while authenticating the user credentials when using Extensible Authentication Protocol (EAP).</exception>
        /// <exception cref="IPSecException">Thrown when an error occurs while dialing the connection when using IPSec.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasException">Thrown when an error occurs while dialing the connection.</exception>
        /// <exception cref="Win32Exception">Thrown when an error occurs while dialing the connection.</exception>
        public Task<RasConnection> ConnectAsync(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();

            return api.DialAsync(new RasDialContext
            {
                PhoneBookPath = PhoneBookPath,
                EntryName = EntryName,
                Credentials = Credentials,
                CancellationToken = cancellationToken,
                Options = Options,
                OnStateChangedCallback = RaiseStateChangedEvent
            });
        }

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="StateChangedEventArgs"/> containing event data.</param>
        protected void RaiseStateChangedEvent(StateChangedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            {
                RaiseEvent(StateChanged, e);
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(new ErrorEventArgs(ex));
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                api.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}