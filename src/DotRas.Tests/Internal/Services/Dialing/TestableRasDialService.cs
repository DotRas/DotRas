using System;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Dialing;

namespace DotRas.Tests.Internal.Services.Dialing
{
    internal class TestableRasDialService : RasDialService
    {
        public bool CancelledAttempt { get; private set; }

        public Action OnInitializeCallback { get; set; }

        public TestableRasDialService(IRasApi32 api, IRasHangUp hangUpService,
            IRasDialExtensionsBuilder extensionsBuilder, IRasDialParamsBuilder paramsBuilder,
            IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler, IMarshaller marshaller)
            : base(api, hangUpService, extensionsBuilder, paramsBuilder, exceptionPolicy, callbackHandler, marshaller)
        {
        }

        public void FlagAsBusy()
        {
            IsBusy = true;
        }

        public void SetCompletionSource(TaskCompletionSource<RasConnection> value)
        {
            CompletionSource = value;
        }

        protected override TaskCompletionSource<RasConnection> CreateCompletionSource()
        {
            var result = CompletionSource ?? base.CreateCompletionSource();
            OnInitializeCallback?.Invoke();

            return result;
        }

        protected override void CancelAttemptIfBusy()
        {
            CancelledAttempt = true;
            base.CancelAttemptIfBusy();
        }

        public void SimulateDialCompleted(RasDialContext context)
        {
            OnDialCompletedCallback(context);
        }

        public void SimulateCancellationRequested(RasDialContext context)
        {
            OnCancellationRequestedCallback(context);
        }
    }
}