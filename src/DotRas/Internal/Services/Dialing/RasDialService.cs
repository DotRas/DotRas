using System;
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
        private readonly object syncRoot = new object();

        private readonly IRasApi32 api;
        private readonly IRasDialExtensionsBuilder extensionsBuilder;
        private readonly IRasDialParamsBuilder paramsBuilder;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IRasDialCallbackHandler callbackHandler;
        private readonly ITaskCompletionSourceFactory completionSourceFactory;
        private readonly RasDialFunc2 callback;

        public bool IsBusy { get; private set; }

        public RasDialService(IRasApi32 api, IRasDialExtensionsBuilder extensionsBuilder, IRasDialParamsBuilder paramsBuilder, IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler, ITaskCompletionSourceFactory completionSourceFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.extensionsBuilder = extensionsBuilder ?? throw new ArgumentNullException(nameof(extensionsBuilder));
            this.paramsBuilder = paramsBuilder ?? throw new ArgumentNullException(nameof(paramsBuilder));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.callbackHandler = callbackHandler ?? throw new ArgumentNullException(nameof(callbackHandler));
            this.completionSourceFactory = completionSourceFactory ?? throw new ArgumentNullException(nameof(completionSourceFactory));

            callback = callbackHandler.OnCallback;
        }

        public Task<RasConnection> DialAsync(RasDialContext context)
        {
            GuardMustNotBeDisposed();
            GuardMustNotAlreadyBeBusy();

            lock (syncRoot)
            {
                GuardMustNotAlreadyBeBusy();

                var completionSource = CreateCompletionSource();
                if (completionSource == null)
                {
                    throw new InvalidOperationException("The completion source was not created.");
                }

                InitializeCallbackHandler(completionSource, context);
                BeginDial(context);

                return completionSource.Task;
            }
        }

        private ITaskCompletionSource<RasConnection> CreateCompletionSource()
        {
            return completionSourceFactory.Create<RasConnection>();
        }

        private void InitializeCallbackHandler(ITaskCompletionSource<RasConnection> completionSource, RasDialContext context)
        {
            callbackHandler.Initialize(completionSource, context.OnStateChangedCallback, SetNotBusy, context.CancellationToken);
        }

        private void BeginDial(RasDialContext context)
        {
            var lphRasConn = IntPtr.Zero;

            try
            {
                SetBusy();

                var rasDialExtensions = ConvertToRasDialExtensions(context);
                var rasDialParams = ConvertToRasDialParams(context);

                var ret = api.RasDial(ref rasDialExtensions, context.PhoneBookPath, ref rasDialParams, NotifierType.RasDialFunc2, callback, out lphRasConn);
                if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }

                callbackHandler.SetHandle(lphRasConn);
            }
            catch (Exception)
            {
                SetNotBusy();
                throw;
            }
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
                callbackHandler.DisposeIfNecessary();
            }

            base.Dispose(disposing);
        }
    }
}