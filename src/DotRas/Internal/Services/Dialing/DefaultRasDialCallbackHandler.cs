using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
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
        private readonly ITaskCancellationSourceFactory cancellationSourceFactory;

        private ITaskCancellationSource cancellationSource;
        private CancellationToken cancellationToken;
        private ITaskCompletionSource<RasConnection> completionSource;
        private Action<StateChangedEventArgs> onStateChangedCallback;
        private Action onCompletedCallback;
        
        public bool Completed { get; private set; }
        public bool Initialized { get; private set; }

        #endregion

        public DefaultRasDialCallbackHandler(IRasHangUp rasHangUp, IRasEnumConnections rasEnumConnections, IExceptionPolicy exceptionPolicy, IValueWaiter<IntPtr> handle, ITaskCancellationSourceFactory cancellationSourceFactory)
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
                cancellationSource?.Dispose();
                handle.Dispose();
            }

            base.Dispose(disposing);
        }

        private RasDialContext context;

        public void Initialize(RasDialContext context, ITaskCompletionSource<RasConnection> completionSource, Action<StateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken)
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

            this.context = context;

            GuardMustNotBeDisposed();

            lock (syncRoot)
            {
                cancellationSource?.Dispose();
                cancellationSource = cancellationSourceFactory.Create(cancellationToken);

                this.cancellationToken = cancellationSource.Token;
                this.cancellationToken.Register(HangUpConnection);
                
                this.completionSource = completionSource;
                this.onStateChangedCallback = onStateChangedCallback;
                this.onCompletedCallback = onCompletedCallback;

                handle.Reset();

                Completed = false;
                Initialized = true;
            }
        }

        private void HangUpConnection()
        {
            WaitForHandleToBeTransferred();

            rasHangUp.UnsafeHangUp(handle.Value, false);
        }

        private int subEntryId;

        public bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hRasConn, uint message, RasConnectionState connectionState, int dwError, int dwExtendedError)
        {
            GuardMustNotBeDisposed();
            GuardMustBeInitialized();

            WaitForHandleToBeTransferred();

            try
            {
                GuardRequestShouldNotBeCancelled();
                GuardErrorCodeMustBeZero(dwError);

                if (connectionState == RasConnectionState.InvokeEapUI)
                {
                    subEntryId = dwSubEntry;

                    var t = new Thread(LoadEapUserIdentity);
                    t.Start();
                }
                else
                {
                    ExecuteStateChangedCallback(connectionState);

                    if (HasConnectionCompleted(connectionState))
                    {
                        SetConnectionResult();
                    }
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
                if (Completed)
                {
                    RunPostCompleted();
                }
            }

            return !Completed;
        }

        private void LoadEapUserIdentity()
        {
            var rasDialExtensions = new NativeMethods.RASDIALEXTENSIONS
            {
                dwSize = Marshal.SizeOf<NativeMethods.RASDIALEXTENSIONS>()
            };

            var lpRasDialExtensions = Marshal.AllocHGlobal(rasDialExtensions.dwSize);
            Marshal.StructureToPtr(rasDialExtensions, lpRasDialExtensions, true);

            var ret = ServiceLocator.Default.GetRequiredService<IRasApi32>().RasInvokeEapUI(
                handle.Value, subEntryId, lpRasDialExtensions, IntPtr.Zero);
            if (ret == SUCCESS)
            {
                //var rasDialParams = new NativeMethods.RASDIALPARAMS
                //{
                //    dwSize = Marshal.SizeOf<NativeMethods.RASDIALPARAMS>(),
                //    szEntryName = context.EntryName
                //};

                //var lpRasDialParams = Marshal.AllocHGlobal(rasDialParams.dwSize);
                //Marshal.StructureToPtr(rasDialParams, lpRasDialParams, true);

                //var rasDialExtensions = Marshal.PtrToStructure<NativeMethods.RASDIALEXTENSIONS>(context.LpDialExtensions);
                //rasDialExtensions.dwfOptions = Ras.RDEOPT.None;            
                //Marshal.StructureToPtr(rasDialExtensions, context.LpDialExtensions, true);

                var ptr = handle.Value;
                var callback = context.Callback;
                var phoneBookPath = context.PhoneBookPath;

                //var rasDialExtensions = Marshal.PtrToStructure<NativeMethods.RASDIALEXTENSIONS>(context.LpDialExtensions);

                //var rasDialParams = new NativeMethods.RASDIALPARAMS
                //{
                //    dwSize = Marshal.SizeOf<NativeMethods.RASDIALPARAMS>(),
                //    dwSubEntry = 1,
                //    szEntryName = context.EntryName
                //};

                //var lpRasDialParams = Marshal.AllocHGlobal(rasDialParams.dwSize);
                //Marshal.StructureToPtr(rasDialParams, lpRasDialParams, true);

                ret = ServiceLocator.Default.GetRequiredService<IRasApi32>()
                    .RasDial(
                        context.LpDialExtensions,
                        phoneBookPath, 
                        context.LpDialParams,
                        Ras.NotifierType.None, 
                        callback, 
                        out ptr);

                //if (ret != SUCCESS)
                //{
                //    throw exceptionPolicy.Create(ret);
                //}
            }
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

            completionSource.SetResult(connection);
            FlagRequestAsCompleted();
        }

        protected virtual RasConnection CreateConnection(IntPtr handle)
        {
            return rasEnumConnections.EnumerateConnections().SingleOrDefault(o => o.Handle == handle);
        }

        private void SetExceptionResult(Exception exception)
        {
            completionSource.SetException(exception);
            FlagRequestAsCompleted();
        }

        private void FlagRequestAsCompleted()
        {
            Completed = true;
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