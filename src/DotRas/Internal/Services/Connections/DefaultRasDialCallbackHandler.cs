using System;
using System.Linq;
using System.Threading;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class DefaultRasDialCallbackHandler : DisposableObject, IRasDialCallbackHandler
    {
        #region Fields

        private readonly object syncRoot = new object();

        private readonly IRasHangUp rasHangUp;
        private readonly IRasEnumConnections rasEnumConnections;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IValueWaiter<RasHandle> handle;
        private readonly ITaskCancellationSourceFactory cancellationSourceFactory;

        private ITaskCancellationSource cancellationSource;
        private CancellationToken cancellationToken;
        private ITaskCompletionSource<RasConnection> completionSource;
        private Action<DialerStateChangedEventArgs> onStateChangedCallback;
        private Action onCompletedCallback;
        private bool completed;

        public bool Initialized { get; private set; }

        #endregion

        public DefaultRasDialCallbackHandler(IRasHangUp rasHangUp, IRasEnumConnections rasEnumConnections, IExceptionPolicy exceptionPolicy, IValueWaiter<RasHandle> handle, ITaskCancellationSourceFactory cancellationSourceFactory)
        {
            this.rasHangUp = rasHangUp ?? throw new ArgumentNullException(nameof(rasHangUp));
            this.rasEnumConnections = rasEnumConnections ?? throw new ArgumentNullException(nameof(rasEnumConnections));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
            this.cancellationSourceFactory = cancellationSourceFactory ?? throw new ArgumentNullException(nameof(cancellationSourceFactory));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancellationSource.DisposeIfNecessary();
                handle.DisposeIfNecessary();
            }

            base.Dispose(disposing);
        }

        public void Initialize(ITaskCompletionSource<RasConnection> completionSource, Action<DialerStateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken)
        {
            if (completionSource == null)
            {
                throw new ArgumentNullException(nameof(completionSource));
            }

            if (onStateChangedCallback == null)
            {
                throw new ArgumentNullException(nameof(onStateChangedCallback));
            }

            if (onCompletedCallback == null)
            {
                throw new ArgumentNullException(nameof(onCompletedCallback));
            }

            GuardMustNotBeDisposed();

            lock (syncRoot)
            {
                cancellationSource.DisposeIfNecessary();                
                cancellationSource = cancellationSourceFactory.Create(cancellationToken);

                this.cancellationToken = cancellationSource.Token;
                this.cancellationToken.Register(HangUpConnection);
                
                this.completionSource = completionSource;
                this.onStateChangedCallback = onStateChangedCallback;
                this.onCompletedCallback = onCompletedCallback;

                handle.Reset();
                completed = false;

                Initialized = true;
            }
        }

        private void HangUpConnection()
        {
            WaitForHandleToBeTransferred();

            rasHangUp.HangUp(handle.Value, CancellationToken.None);
        }

        public bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hRasConn, uint message, RasConnectionState connectionState, int dwError, int dwExtendedError)
        {
            GuardMustNotBeDisposed();
            GuardMustBeInitialized();

            WaitForHandleToBeTransferred();

            try
            {
                GuardRequestShouldNotBeCancelled();
                GuardErrorCodeMustBeZero(dwError);

                ExecuteStateChangedCallback(connectionState);

                if (HasConnectionCompleted(connectionState))
                {
                    SetConnectionResult();
                }
            }
            catch (OperationCanceledException operationCanceledEx)
            {
                SetExceptionResult(operationCanceledEx);
            }
            catch (Exception ex)
            {
                HangUpConnection();
                SetExceptionResult(ex);
            }
            finally
            {
                if (HasCompleted())
                {
                    RunPostCompleted();
                }
            }

            return !completed;
        }

        private void ExecuteStateChangedCallback(RasConnectionState connectionState)
        {
            onStateChangedCallback(new DialerStateChangedEventArgs(connectionState));
        }

        private void GuardErrorCodeMustBeZero(int errorCode)
        {
            if (errorCode != SUCCESS)
            {
                throw exceptionPolicy.Create(errorCode);
            }
        }

        private bool HasConnectionCompleted(RasConnectionState connectionState)
        {
            return connectionState == RasConnectionState.Connected;
        }

        private void RunPostCompleted()
        {
            onCompletedCallback();
        }

        private bool HasCompleted()
        {
            return completed;
        }

        private void SetConnectionResult()
        {
            var connection = CreateConnection(handle.Value);
            if (connection == null)
            {
                throw new InvalidOperationException("The connection was not created.");
            }

            completionSource.SetResultAsynchronously(connection);
            FlagRequestAsCompleted();
        }

        protected virtual RasConnection CreateConnection(RasHandle handle)
        {
            return rasEnumConnections.EnumerateConnections().SingleOrDefault(o => o.Handle.Equals(handle));
        }

        private void SetExceptionResult(Exception exception)
        {
            completionSource.SetExceptionAsynchronously(exception);
            FlagRequestAsCompleted();
        }

        private void FlagRequestAsCompleted()
        {
            completed = true;
        }

        private void GuardMustBeInitialized()
        {
            lock (syncRoot)
            {
                if (!Initialized)
                {
                    throw new InvalidOperationException("The callback handler has not been initialized.");
                }
            }
        }

        private void WaitForHandleToBeTransferred()
        {
            handle.WaitForValue(cancellationSource.Token);
        }

        public void SetHandle(RasHandle value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            GuardHandleMustBeNull();
            handle.Set(value);
        }        

        private void GuardHandleMustBeNull()
        {
            if (handle.IsSet)
            {
                throw new InvalidOperationException("The handle has already been set.");
            }
        }

        private void GuardRequestShouldNotBeCancelled()
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}