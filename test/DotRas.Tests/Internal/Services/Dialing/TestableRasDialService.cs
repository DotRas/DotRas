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

        public TestableRasDialService(IRasApi32 api, IRasHangUp hangUpService, IRasDialExtensionsBuilder extensionsBuilder, IRasDialParamsBuilder paramsBuilder, IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler)
            : base(api, hangUpService, extensionsBuilder, paramsBuilder, exceptionPolicy, callbackHandler)
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
            return CompletionSource ?? base.CreateCompletionSource();
        }

        protected override void CancelAttemptIfBusy()
        {
            CancelledAttempt = true;
            base.CancelAttemptIfBusy();
        }
    }
}