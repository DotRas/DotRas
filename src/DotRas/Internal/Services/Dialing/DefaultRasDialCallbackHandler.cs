using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Dialing
{
    internal class DefaultRasDialCallbackHandler : DisposableObject, IRasDialCallbackHandler
    {
        #region Fields and Properties

        private readonly object syncRoot = new object();

        private readonly IRasHangUp rasHangUp;
        private readonly IRasEnumConnections rasEnumConnections;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IValueWaiter<IntPtr> handle;

        private CancellationToken cancellationToken;
        private TaskCompletionSource<RasConnection> completionSource;
        private Action<StateChangedEventArgs> onStateChangedCallback;
        private Action onCompletedCallback;
        
        public bool Completed { get; private set; }

        public bool HasEncounteredErrors { get; private set; }
        
        public bool Initialized { get; private set; }

        #endregion

        public DefaultRasDialCallbackHandler(IRasHangUp rasHangUp, IRasEnumConnections rasEnumConnections, IExceptionPolicy exceptionPolicy, IValueWaiter<IntPtr> handle)
        {
            this.rasHangUp = rasHangUp ?? throw new ArgumentNullException(nameof(rasHangUp));
            this.rasEnumConnections = rasEnumConnections ?? throw new ArgumentNullException(nameof(rasEnumConnections));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                handle.Dispose();
            }

            base.Dispose(disposing);
        }

        public void Initialize(TaskCompletionSource<RasConnection> completionSource, Action<StateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken)
        {
            if (completionSource == null)
            {
                throw new ArgumentNullException(nameof(completionSource));
            }
            else if (onStateChangedCallback == null)
            {
                throw new ArgumentNullException(nameof(onStateChangedCallback));
            }
            else if (onCompletedCallback == null)
            {
                throw new ArgumentNullException(nameof(onCompletedCallback));
            }

            GuardMustNotBeDisposed();

            lock (syncRoot)
            {
                this.cancellationToken = cancellationToken;                
                this.completionSource = completionSource;
                this.onStateChangedCallback = onStateChangedCallback;
                this.onCompletedCallback = onCompletedCallback;
                 
                handle.Reset();

                Completed = false;
                Initialized = true;
            }
        }

        public bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hrasconn, uint message, RasConnectionState rascs, int dwError, int dwExtendedError)
        {
            GuardMustNotBeDisposed();
            GuardMustBeInitialized();

            WaitForHandleToBeTransferred();

            try
            {
                GuardRequestShouldNotBeCancelled();
                GuardErrorCodeMustBeZero(dwError);

                ExecuteStateChangedCallback(rascs);

                if (HasConnectionCompleted(rascs))
                {
                    SetConnectionResult();
                }
            }
            catch (Exception ex)
            {
                HangUpConnection();
                SetExceptionResult(ex);
            }            

            return !Completed;
        }

        private void HangUpConnection()
        {
            rasHangUp.UnsafeHangUp(handle.Value, false, CancellationToken.None);
        }

        private void ExecuteStateChangedCallback(RasConnectionState connectionState)
        {
            onStateChangedCallback(new StateChangedEventArgs(connectionState));
        }

        private void GuardErrorCodeMustBeZero(int errorCode)
        {
            if (errorCode != SUCCESS)
            {
                throw exceptionPolicy.Create(errorCode);
            }
        }

        private static bool HasConnectionCompleted(RasConnectionState connectionState)
        {
            return connectionState == RasConnectionState.Connected;
        }

        private void RunPostCompleted()
        {
            onCompletedCallback();
        }

        private void SetConnectionResult()
        {
            var connection = CreateConnection(handle.Value);
            if (connection == null)
            {
                throw new InvalidOperationException("The connection was not created.");
            }

            FlagRequestAsCompleted();
            RunPostCompleted();

            completionSource.SetResult(connection);
        }

        protected virtual RasConnection CreateConnection(IntPtr handle)
        {
            return rasEnumConnections.EnumerateConnections().SingleOrDefault(o => o.Handle == handle);
        }

        private void SetExceptionResult(Exception exception)
        {
            FlagRequestAsEncounteredErrors();
            FlagRequestAsCompleted();

            RunPostCompleted();

            completionSource.SetException(exception);
        }

        private void FlagRequestAsCompleted()
        {
            Completed = true;
        }

        private void FlagRequestAsEncounteredErrors()
        {
            HasEncounteredErrors = true;
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
            handle.WaitForValue(cancellationToken);
        }

        public void SetHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            GuardHandleMustBeNull();
            this.handle.Set(handle);
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