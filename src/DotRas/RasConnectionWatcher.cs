using System;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.IoC;

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
        /// Gets a value indicating whether this instance is actively watching for connection changes.
        /// </summary>
        public bool IsActive => api.IsActive;

        private IRasConnection connection;

        /// <summary>
        /// Gets or sets the connection to watch for changes.
        /// </summary>
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

        /// <summary>
        /// Occurs when an exception occurs while processing the notification.
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;

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
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after Dispose has been called.</exception>
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
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after Dispose has been called.</exception>
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

        private void RaiseConnectedEvent(RasConnectionEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            {
                Connected?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(new ErrorEventArgs(ex));
            }
        }

        private void RaiseDisconnectedEvent(RasConnectionEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            {
                Disconnected?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(new ErrorEventArgs(ex));
            }
        }

        private void RaiseErrorEvent(ErrorEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            {
                Error?.Invoke(this, e);
            }
            catch (Exception)
            {
                // Swallow any errors which occur while processing the error event.
            }
        }
    }
}