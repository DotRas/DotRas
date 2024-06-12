using DotRas.Internal.Abstractions.Services;
using System;
using static DotRas.Internal.Interop.EapHostError;

namespace DotRas.Internal.Policies {
    internal class RasDialCallbackExceptionPolicy : DefaultExceptionPolicy {
        public RasDialCallbackExceptionPolicy(IRasGetErrorString rasGetErrorString)
            : base(rasGetErrorString) { }

        public override Exception Create(int error) {
            return ShouldGetMessageFromEap(error) ? CreateExceptionFromEap(error) : base.Create(error);
        }

        private bool ShouldGetMessageFromEap(int error) => error == EAP_E_USER_NAME_PASSWORD_REJECTED;

        private Exception CreateExceptionFromEap(int error) => new EapException(error, "Authenticator rejected user credentials for authentication.");
    }
}
