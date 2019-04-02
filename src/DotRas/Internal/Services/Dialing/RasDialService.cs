using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.WinError;

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
        private readonly ITaskCompletionSourceFactory completionSourceFactory;
        private readonly ITaskCancellationSourceFactory cancellationSourceFactory;
        private readonly RasDialFunc2 callback;

        public ITaskCancellationSource CancellationSource { get; private set; }
        public ITaskCompletionSource<RasConnection> CompletionSource { get; private set; }
        public bool IsBusy { get; private set; }

        public RasDialService(IRasApi32 api, IRasHangUp hangUpService, IRasDialExtensionsBuilder extensionsBuilder, IRasDialParamsBuilder paramsBuilder, IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler, ITaskCompletionSourceFactory completionSourceFactory, ITaskCancellationSourceFactory cancellationSourceFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.hangUpService = hangUpService ?? throw new ArgumentNullException(nameof(hangUpService));
            this.extensionsBuilder = extensionsBuilder ?? throw new ArgumentNullException(nameof(extensionsBuilder));
            this.paramsBuilder = paramsBuilder ?? throw new ArgumentNullException(nameof(paramsBuilder));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.callbackHandler = callbackHandler ?? throw new ArgumentNullException(nameof(callbackHandler));
            this.completionSourceFactory = completionSourceFactory ?? throw new ArgumentNullException(nameof(completionSourceFactory));
            this.cancellationSourceFactory = cancellationSourceFactory ?? throw new ArgumentNullException(nameof(cancellationSourceFactory));

            callback = callbackHandler.OnCallback;
        }

        public Task<RasConnection> DialAsync(RasDialContext context)
        {
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

        private ITaskCompletionSource<RasConnection> CreateCompletionSource()
        {
            var completionSource = completionSourceFactory.Create<RasConnection>();
            if (completionSource == null)
            {
                throw new InvalidOperationException("The completion source was not created.");
            }

            return completionSource;
        }

        private void InitializeCallbackHandler(RasDialContext context)
        {
            callbackHandler.Initialize(context, CompletionSource, context.OnStateChangedCallback, SetNotBusy, CancellationSource.Token);
        }

        private void SetUpCancellationSource(RasDialContext context)
        {
            CancellationSource?.Dispose();
            CancellationSource = cancellationSourceFactory.Create(context.CancellationToken);
        }

        private void BeginDial(RasDialContext context)
        {
            var handle = IntPtr.Zero;

            try
            {
                SetBusy();

                var rasDialExtensions = ConvertToRasDialExtensions(context);
                context.DialExtensions = rasDialExtensions;

                var rasDialParams = ConvertToRasDialParams(context);
                context.DialParams = rasDialParams;

                context.Callback = callback;

                LoadEapUserIdentity(context, ref rasDialExtensions, ref rasDialParams);

                var lpRasDialParams = Marshal.AllocHGlobal(rasDialParams.dwSize);
                Marshal.StructureToPtr(rasDialParams, lpRasDialParams, true);

                var lpRasDialExtensions = Marshal.AllocHGlobal(rasDialExtensions.dwSize);
                Marshal.StructureToPtr(rasDialExtensions, lpRasDialExtensions, true);

                context.LpDialParams = lpRasDialParams;
                context.LpDialExtensions = lpRasDialExtensions;

                var ret = api.RasDial(lpRasDialExtensions, context.PhoneBookPath, lpRasDialParams, NotifierType.RasDialFunc2, callback, out handle);
                if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }

                callbackHandler.SetHandle(handle);
            }
            catch (Exception)
            {
                HangUpIfNecessary(handle);
                SetNotBusy();

                throw;
            }
        }

        private void LoadEapUserIdentity(RasDialContext context, ref RASDIALEXTENSIONS rasDialExtensions, ref RASDIALPARAMS rasDialParams)
        {
            var ret = api.RasGetEapUserIdentity(context.PhoneBookPath, context.EntryName, RASEAPF.None, IntPtr.Zero, out var rasEapCredential);
            if (ret == SUCCESS)
            {
                var rasEapUserIdentity = Marshal.PtrToStructure<RASEAPUSERIDENTITY>(rasEapCredential.Handle);

                rasDialExtensions.RasEapInfo.dwSizeofEapInfo = rasEapUserIdentity.dwSizeofEapInfo;
                rasDialExtensions.RasEapInfo.pbEapInfo = Marshal.UnsafeAddrOfPinnedArrayElement(rasEapUserIdentity.pbEapInfo, 0);
                rasDialParams.szUserName = rasEapUserIdentity.szUserName;
            }
        }

        private void HangUpIfNecessary(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }

            hangUpService.UnsafeHangUp(handle, false);
        }

        private RASDIALEXTENSIONS ConvertToRasDialExtensions(RasDialContext context)
        {
            return extensionsBuilder.Build(context);
        }

        private RASDIALPARAMS ConvertToRasDialParams(RasDialContext context)
        {
            return paramsBuilder.Build(context);
        }

        private void SetBusy()
        {
            IsBusy = true;
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
                if (IsBusy)
                {
                    CancellationSource.Cancel();
                }

                CancellationSource?.Dispose();
                callbackHandler.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}