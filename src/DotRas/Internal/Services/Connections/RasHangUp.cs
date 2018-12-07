using System;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Win32;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.RasError;
using static DotRas.Win32.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasHangUp : IRasHangUp
    {
        private readonly IRasApi32 api;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasHangUp(IRasApi32 api, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public Task HangUpAsync(RasHandle handle, CancellationToken cancellationToken)
        {
            return Task.Run(() => HangUp(handle, cancellationToken));
        }

        public void HangUp(RasHandle handle, CancellationToken cancellationToken)
        {
            if (handle == null)
            {
                throw new ArgumentNullException(nameof(handle));
            }
            else if (handle.IsClosed || handle.IsInvalid)
            {
                throw new ArgumentException("The handle is invalid.", nameof(handle));
            }

            int ret;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                ret = api.RasHangUp(handle);
                if (ShouldThrowExceptionFromReturnCode(ret))
                {
                    throw exceptionPolicy.Create(ret);
                }
            } while (ret == SUCCESS);

            handle.SetHandleAsInvalid();
        }

        private bool ShouldThrowExceptionFromReturnCode(int ret)
        {
            return ret != SUCCESS && ret != ERROR_NO_CONNECTION;
        }
    }
}