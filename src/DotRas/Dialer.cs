using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;

namespace DotRas
{
    /// <summary>
    /// Provides a mechanism to dial a connection.
    /// </summary>
    public sealed class Dialer : DisposableObject
    {
        private readonly IRasDial api;        

        /// <summary>
        /// Occurs when the state changes while dialing a connection.
        /// </summary>
        /// <remarks>
        /// Please note, this event is only raised while a connection is being dialed. It will not be raised if
        /// an active connection has been disconnected outside of an attempt to dial.
        /// </remarks>
        public event EventHandler<DialerStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Gets or sets the name of the entry within the phone book.
        /// </summary>
        public string EntryName { get; set; }

        /// <summary>
        /// Gets the full path (including filename) of the phone book containing the entry.
        /// </summary>
        public string PhoneBook { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public NetworkCredential Credentials { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Dialer"/> class.
        /// </summary>
        public Dialer()
            : this(Container.Default.GetRequiredService<IRasDial>())
        {
        }

        internal Dialer(IRasDial api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        /// <summary>
        /// Dials the connection.
        /// </summary>
        /// <returns>The connection instance.</returns>
        public Connection Dial()
        {
            GuardMustNotBeDisposed();

            return Dial(CancellationToken.None);
        }

        /// <summary>
        /// Dials the connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests while dialing the connection.</param>
        /// <returns>The connection instance.</returns>
        public Connection Dial(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();

            using (var task = DialAsync(cancellationToken))
            {
                return task.Result;
            }
        }

        /// <summary>
        /// Dials the connection asynchronously.
        /// </summary>
        /// <returns>The connection instance.</returns>
        public Task<Connection> DialAsync()
        {
            GuardMustNotBeDisposed();

            return DialAsync(CancellationToken.None);
        }

        /// <summary>
        /// Dials the connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests while dialing the connection.</param>
        /// <returns>The connection instance.</returns>
        public Task<Connection> DialAsync(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();

            return api.DialAsync(new RasDialContext(PhoneBook, EntryName, Credentials, cancellationToken, RaiseDialStateChanged));
        }

        private void RaiseDialStateChanged(DialerStateChangedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            StateChanged?.Invoke(this, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                api.DisposeIfNecessary();
            }

            base.Dispose(disposing);
        }
    }
}