using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;

namespace DotRas
{
    /// <summary>
    /// Provides a mechanism to dial a connection.
    /// </summary>
    public sealed class RasDialer : DisposableObject
    {
        private readonly IRasDial api;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Occurs when the state changes while dialing a connection.
        /// </summary>
        /// <remarks>
        /// Please note, this event is only raised while a connection is being dialed. It will not be raised if
        /// an active connection has been disconnected outside of an attempt to dial.
        /// </remarks>
        public event EventHandler<StateChangedEventArgs> StateChanged;

        /// <summary>
        /// Gets or sets the name of the entry within the phone book.
        /// </summary>
        public string EntryName { get; set; }

        /// <summary>
        /// Gets the full path (including filename) of the phone book containing the entry.
        /// </summary>
        public string PhoneBookPath { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public NetworkCredential Credentials { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RasDialer"/> class.
        /// </summary>
        public RasDialer()
            : this(Container.Default.GetRequiredService<IRasDial>(), Container.Default.GetRequiredService<IFileSystem>())
        {
        }

        internal RasDialer(IRasDial api, IFileSystem fileSystem)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        /// <summary>
        /// Dials the connection.
        /// </summary>
        /// <returns>The connection instance.</returns>
        public RasConnection Dial()
        {
            GuardMustNotBeDisposed();

            return Dial(CancellationToken.None);
        }

        /// <summary>
        /// Dials the connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests while dialing the connection.</param>
        /// <returns>The connection instance.</returns>
        public RasConnection Dial(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();
            ValidateConfigurationForDialAttempt();

            using (var task = DialAsync(cancellationToken))
            {
                return task.Result;
            }
        }

        /// <summary>
        /// Dials the connection asynchronously.
        /// </summary>
        /// <returns>The connection instance.</returns>
        public Task<RasConnection> DialAsync()
        {
            GuardMustNotBeDisposed();

            return DialAsync(CancellationToken.None);
        }

        /// <summary>
        /// Dials the connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests while dialing the connection.</param>
        /// <returns>The connection instance.</returns>
        public Task<RasConnection> DialAsync(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();
            ValidateConfigurationForDialAttempt();

            return api.DialAsync(new RasDialContext(PhoneBookPath, EntryName, Credentials, cancellationToken, RaiseDialStateChanged));
        }

        private void ValidateConfigurationForDialAttempt()
        {
            if (string.IsNullOrWhiteSpace(EntryName))
            {
                throw new RasDialerConfigurationException("The entry name has not been set.");
            }

            if (string.IsNullOrWhiteSpace(PhoneBookPath) || !fileSystem.VerifyFileExists(PhoneBookPath))
            {
                throw new RasDialerConfigurationException("The phone book path has not been set or the file does not exist.");
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