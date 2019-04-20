using System;
using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;

namespace DotRas
{
    /// <summary>
    /// Listens to the remote access service (RAS) change notifications and raises events when connections change.
    /// </summary>
    public class RasConnectionWatcher : RasComponentBase
    {
        #region Fields and Properties

        private readonly IRasConnectionNotification api;

        /// <summary>
        /// Gets a value indicating whether this instance is actively watching for connection changes.
        /// </summary>
        public bool IsActive => api.IsActive;

        private IRasConnection connection;

        /// <summary>
        /// Gets or sets the connection to watch for changes.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The connection has been changed after the <see cref="Dispose"/> method has been called.</exception>
        public IRasConnection Connection
        {
            get => connection;
            set
            {
                if (connection == null && value == null || Equals(connection, value))
                {
                    return;
                }

                connection = value;
                if (IsActive)
                {
                    Restart();
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a new connection has been established.
        /// </summary>
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
            : this(ServiceLocator.Default.GetRequiredService<IRasConnectionNotification>())
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
        /// Starts watching for connection changes.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after the <see cref="Dispose"/> method has been called.</exception>
        public void Start()
        {
            GuardMustNotBeDisposed();

            if (IsActive)
            {
                return;
            }

            api.Subscribe(new RasNotificationContext
            {
                Connection = connection,
                OnConnectedCallback = RaiseConnectedEvent,
                OnDisconnectedCallback = RaiseDisconnectedEvent
            });
        }

        /// <summary>
        /// Stops watching for connection changes.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after the <see cref="Dispose"/> method has been called.</exception>
        public void Stop()
        {
            GuardMustNotBeDisposed();

            if (!IsActive)
            {
                return;
            }

            api.Reset();
        }

        private void Restart()
        {
            Stop();
            Start();
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

        /// <summary>
        /// Raises the <see cref="Connected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="RasConnectionEventArgs"/> containing event data.</param>
        protected void RaiseConnectedEvent(RasConnectionEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            {
                RaiseEvent(Connected, e);
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(new ErrorEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="RasConnectionEventArgs"/> containing event data.</param>
        protected void RaiseDisconnectedEvent(RasConnectionEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            {
                RaiseEvent(Disconnected, e);
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(new ErrorEventArgs(ex));
            }
        }
    }
}