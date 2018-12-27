using System;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.EapHostError;

namespace DotRas.Internal.Policies
{
    internal class RasDialCallbackExceptionPolicy : DefaultExceptionPolicy
    {
        public RasDialCallbackExceptionPolicy(IRasGetErrorString rasGetErrorString)
            : base(rasGetErrorString)
        {
        }

        public override Exception Create(int error)
        {
            if (ShouldGetMessageFromEap(error))
            {
                return CreateExceptionFromEap(error);
            }

            return base.Create(error);
        }

        private bool ShouldGetMessageFromEap(int error)
        {
            return error == EAP_E_USER_NAME_PASSWORD_REJECTED;
        }

        private Exception CreateExceptionFromEap(int error)
        {
            return new EapException(error, "Authenticator rejected user credentials for authentication.");
        }
    }
}