using System;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Composition;

namespace DotRas
{
    /// <summary>
    /// Listens to the remote access service (RAS) change notifications and raises events when connections change.
    /// </summary>
    public class RasConnectionWatcher : DisposableObject
    {
        #region Fields and Properties

        private readonly IRasConnectionNotification api;

        /// <summary>
        /// Gets a value indicating whether this instance is watching for connection changes.
        /// </summary>
        public bool IsActive { get; } = false;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a new connection has been established.
        /// </summary>
        /// <remarks>This event will only be raised if the watcher is watching for any connections.</remarks>
        public event EventHandler<RasConnectionEventArgs> Connected;

        /// <summary>
        /// Occurs when a connection has disconnected.
        /// </summary>
        public event EventHandler<RasConnectionEventArgs> Disconnected;

        #endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionWatcher"/> class.
        /// </summary>
        public RasConnectionWatcher()
            : this(CompositionRoot.Default.GetRequiredService<IRasConnectionNotification>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionWatcher"/> class.
        /// </summary>
        internal RasConnectionWatcher(IRasConnectionNotification api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        /// <summary>
        /// Watch any connections for changes.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after Dispose has been called.</exception>
        public void WatchAnyConnections()
        {
            GuardMustNotBeDisposed();

            Subscribe(null);
        }

        /// <summary>
        /// Watch a specific connection for changes.
        /// </summary>
        /// <param name="connection">The connection to watch.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection"/> is a null reference.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after Dispose has been called.</exception>
        public void WatchConnection(RasConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            GuardMustNotBeDisposed();
            
            Subscribe(connection);
        }

        private void Subscribe(IRasConnection connection)
        {
            api.Subscribe(new RasNotificationContext
            {
                Connection = connection,
                OnConnectedCallback = RaiseConnectedEvent,
                OnDisconnectedCallback = RaiseDisconnectedEvent
            });
        }

        /// <summary>
        /// Stops the watcher from watching for connection changes.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after Dispose has been called.</exception>
        public void Stop()
        {
            GuardMustNotBeDisposed();

            api.Dispose();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            api?.Dispose();

            base.Dispose(disposing);
        }

        private void RaiseConnectedEvent(RasConnectionEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            Connected?.Invoke(this, e);
        }

        private void RaiseDisconnectedEvent(RasConnectionEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            Disconnected?.Invoke(this, e);
        }
    }
}