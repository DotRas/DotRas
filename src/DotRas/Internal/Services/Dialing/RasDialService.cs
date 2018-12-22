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
        private readonly IStructFactory structFactory;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IRasDialCallbackHandler callbackHandler;
        private readonly ITaskCompletionSourceFactory completionSourceFactory;
        private readonly RasDialFunc2 callback;

        public bool IsBusy { get; private set; }

        public RasDialService(IRasApi32 api, IStructFactory structFactory, IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler, ITaskCompletionSourceFactory completionSourceFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
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
            RasHandle lphRasConn = null;

            try
            {
                SetBusy();

                var rasDialExtensions = ConvertToRasDialExtensions();
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

                lphRasConn?.Dispose();
                throw;
            }
        }

        private RASDIALEXTENSIONS ConvertToRasDialExtensions()
        {
            return structFactory.Create<RASDIALEXTENSIONS>();
        }

        private RASDIALPARAMS ConvertToRasDialParams(RasDialContext context)
        {
            var rasDialParams = structFactory.Create<RASDIALPARAMS>();
            rasDialParams.szEntryName = context.EntryName;
            rasDialParams.dwIfIndex = context.InterfaceIndex;

            if (context.Credentials != null)
            {
                rasDialParams.szUserName = context.Credentials.UserName;
                rasDialParams.szPassword = context.Credentials.Password;
                rasDialParams.szDomain = context.Credentials.Domain;
            }            

            return rasDialParams;
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