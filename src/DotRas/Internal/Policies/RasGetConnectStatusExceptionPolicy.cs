using System;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Policies
{
    internal class RasGetConnectStatusExceptionPolicy : DefaultExceptionPolicy
    {
        public RasGetConnectStatusExceptionPolicy(IRasGetErrorString rasGetErrorString)
            : base(rasGetErrorString)
        {
        }

        public bool TranslatedToNoConnection { get; private set; }

        public override Exception Create(int error)
        {
            if (ShouldTranslateToNoConnection(error))
            {
                TranslatedToNoConnection = true;

                error = ERROR_NO_CONNECTION;
            }

            return base.Create(error);
        }

        private bool ShouldTranslateToNoConnection(int error)
        {
            return error == ERROR_INVALID_HANDLE;
        }
    }
}