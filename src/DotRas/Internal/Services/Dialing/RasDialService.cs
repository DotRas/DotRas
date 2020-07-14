using System;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.WinError;

#pragma warning disable S1854 // False positive.

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialService : DisposableObject, IRasDial
    {
        private readonly IRasApi32 api;
        private readonly IRasHangUp hangUpService;
        private readonly IRasDialExtensionsBuilder extensionsBuilder;
        private readonly IRasDialParamsBuilder paramsBuilder;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IRasDialCallbackHandler callbackHandler;
        private readonly IMarshaller marshaller;
        private readonly RasDialFunc2 callback;

        public CancellationTokenSource CancellationSource { get; protected set; }
        public TaskCompletionSource<RasConnection> CompletionSource { get; protected set; }
        public bool IsBusy { get; protected set; }

        public RasDialService(IRasApi32 api, IRasHangUp hangUpService, IRasDialExtensionsBuilder extensionsBuilder, IRasDialParamsBuilder paramsBuilder, IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler, IMarshaller marshaller)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.hangUpService = hangUpService ?? throw new ArgumentNullException(nameof(hangUpService));
            this.extensionsBuilder = extensionsBuilder ?? throw new ArgumentNullException(nameof(extensionsBuilder));
            this.paramsBuilder = paramsBuilder ?? throw new ArgumentNullException(nameof(paramsBuilder));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.callbackHandler = callbackHandler ?? throw new ArgumentNullException(nameof(callbackHandler));
            this.marshaller = marshaller ?? throw new ArgumentNullException(nameof(marshaller));

            callback = callbackHandler.OnCallback;
        }

        public Task<RasConnection> DialAsync(RasDialContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            GuardMustNotBeDisposed();
            GuardMustNotAlreadyBeBusy();

            lock (SyncRoot)
            {
                GuardMustNotAlreadyBeBusy();

                CompletionSource = CreateCompletionSource();
                SetUpCancellationSource(context);

                InitializeCallbackHandler(context);
                BeginDial(context);

                return CompletionSource.Task;
            }
        }

        protected virtual TaskCompletionSource<RasConnection> CreateCompletionSource()
        {
            return new TaskCompletionSource<RasConnection>();
        }

        private void InitializeCallbackHandler(RasDialContext context)
        {
            callbackHandler.Initialize(CompletionSource, context.OnStateChangedCallback, () => OnDialCompletedCallback(context), CancellationSource.Token);
        }

        private void SetUpCancellationSource(RasDialContext context)
        {
            CancellationSource?.Dispose();
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);

            // Ensures that the connection can be cancelled even if the callback is stuck.
            CancellationSource.Token.Register(() => OnCancellationRequestedCallback(context));
        }

        protected void OnCancellationRequestedCallback(RasDialContext context)
        {
            if (!IsBusy)
            {
                return;
            }

            HangUpIfNecessary(context);
            OnDialCompletedCallback(context);

            CancelCompletionSourceIfNecessary();
        }

        protected virtual void CancelCompletionSourceIfNecessary()
        {
            var task = CompletionSource?.Task;
            if (task == null || task.IsFaulted || task.IsCompleted || task.IsCanceled)
            {
                return;
            }

            CompletionSource.SetCanceled();
        }

        private void BeginDial(RasDialContext context)
        {
            try
            {
                SetBusy();

                var rasDialExtensions = ConvertToRasDialExtensions(context);
                var rasDialParams = ConvertToRasDialParams(context);

                var handle = IntPtr.Zero;

                try
                {
                    var ret = api.RasDial(ref rasDialExtensions, context.PhoneBookPath, ref rasDialParams, NotifierType.RasDialFunc2, callback, out handle);
                    if (ret != SUCCESS)
                    {
                        throw exceptionPolicy.Create(ret);
                    }
                }
                finally
                {
                    context.Handle = handle;
                }

                callbackHandler.SetHandle(context.Handle);
            }
            catch (Exception)
            {
                HangUpIfNecessary(context);
                SetNotBusy();

                throw;
            }
        }

        private void HangUpIfNecessary(RasDialContext context)
        {
            if (context.Handle == IntPtr.Zero)
            {
                return;
            }

            hangUpService.UnsafeHangUp(context.Handle, false, CancellationToken.None);
        }

        private RASDIALEXTENSIONS ConvertToRasDialExtensions(RasDialContext context)
        {
            var result = extensionsBuilder.Build(context);
            context.RasDialExtensions = result;

            return result;
        }

        private RASDIALPARAMS ConvertToRasDialParams(RasDialContext context)
        {
            var result = paramsBuilder.Build(context);
            context.RasDialParams = result;

            return result;
        }

        private void SetBusy()
        {
            IsBusy = true;
        }

        protected void OnDialCompletedCallback(RasDialContext context)
        {
            marshaller.FreeHGlobalIfNeeded(context.RasDialExtensions.RasEapInfo.pbEapInfo);
            SetNotBusy();
        }

        private void SetNotBusy()
        {
            IsBusy = false;
        }

        private void GuardMustNotAlreadyBeBusy()
        {
            if (IsBusy)
            {
                throw new InvalidOperationException("A connection is already being dialed.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CancelAttemptIfBusy();

                CancellationSource?.Dispose();
                callbackHandler.Dispose();
            }

            base.Dispose(disposing);
        }

        protected virtual void CancelAttemptIfBusy()
        {
            if (IsBusy)
            {
                CancellationSource?.Cancel();
            }
        }
    }
}