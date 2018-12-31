using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;
using Container = DotRas.Internal.DependencyInjection.Container;

namespace DotRas
{
    /// <summary>
    /// Provides a mechanism to establish a connection to a remote network.
    /// </summary>
    public sealed class RasDialer : DisposableObject
    {
        #region Fields and Properties

        private readonly IRasDial api;
        private readonly IFileSystem fileSystem;
        private readonly IPhoneBookEntryValidator validator;

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
        public RasDialer() : this(
                Container.Default.GetRequiredService<IRasDial>(), 
                Container.Default.GetRequiredService<IFileSystem>(), 
                Container.Default.GetRequiredService<IPhoneBookEntryValidator>())
        {
        }

        internal RasDialer(IRasDial api, IFileSystem fileSystem, IPhoneBookEntryValidator validator)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Connects to the remote network.
        /// </summary>
        /// <returns>The connection instance.</returns>
        /// <exception cref="EapException">Thrown when an error occurs while authenticating the user credentials when using Extensible Authentication Protocol (EAP).</exception>
        /// <exception cref="FileNotFoundException">Thrown if the <see cref="PhoneBookPath"/> file does not exist.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasEntryNotFoundException">Thrown if the <see cref="EntryName"/> within the phone book specified does not exist.</exception>
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
        /// <exception cref="FileNotFoundException">Thrown if the <see cref="PhoneBookPath"/> file does not exist.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasEntryNotFoundException">Thrown if the <see cref="EntryName"/> within the phone book specified does not exist.</exception>
        /// <exception cref="RasException">Thrown when an error occurs while dialing the connection.</exception>
        /// <exception cref="Win32Exception">Thrown when an error occurs while dialing the connection.</exception>
        public RasConnection Connect(CancellationToken cancellationToken)
        {
            using (var task = ConnectAsync(cancellationToken))
            {
                return task.Result;
            }
        }

        /// <summary>
        /// Connects to the remote network asynchronously.
        /// </summary>
        /// <returns>The connection instance.</returns>
        /// <exception cref="EapException">Thrown when an error occurs while authenticating the user credentials when using Extensible Authentication Protocol (EAP).</exception>
        /// <exception cref="FileNotFoundException">Thrown if the <see cref="PhoneBookPath"/> file does not exist.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasEntryNotFoundException">Thrown if the <see cref="EntryName"/> within the phone book specified does not exist.</exception>
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
        /// <exception cref="FileNotFoundException">Thrown if the <see cref="PhoneBookPath"/> file does not exist.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the object is used after <see cref="Dispose"/> has been called.</exception>
        /// <exception cref="OperationCanceledException">The operation has been cancelled.</exception>
        /// <exception cref="RasEntryNotFoundException">Thrown if the <see cref="EntryName"/> within the phone book specified does not exist.</exception>
        /// <exception cref="RasException">Thrown when an error occurs while dialing the connection.</exception>
        /// <exception cref="Win32Exception">Thrown when an error occurs while dialing the connection.</exception>
        public Task<RasConnection> ConnectAsync(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();
            ValidateConfigurationPriorToDialAttempt();

            return api.DialAsync(new RasDialContext
            {
                PhoneBookPath = PhoneBookPath,
                EntryName = EntryName,
                Credentials = Credentials,
                CancellationToken = cancellationToken,
                Options = Options,
                OnStateChangedCallback = RaiseDialStateChanged
            });
        }        

        private void ValidateConfigurationPriorToDialAttempt()
        {
            if (!string.IsNullOrWhiteSpace(PhoneBookPath) && !fileSystem.VerifyFileExists(PhoneBookPath))
            {
                throw new FileNotFoundException("The file does not exist.", PhoneBookPath);
            }

            if (string.IsNullOrWhiteSpace(EntryName) || !validator.VerifyEntryExists(EntryName, PhoneBookPath))
            {
                throw new RasEntryNotFoundException("The entry does not exist within the phone book specified.", EntryName, PhoneBookPath);
            }
        }

        private void RaiseDialStateChanged(StateChangedEventArgs e)
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