using System;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasHangUpService : IRasHangUp
    {
        private readonly IRasApi32 api;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasHangUpService(IRasApi32 api, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public Task HangUpAsync(IRasConnection connection, bool closeAllReferences, CancellationToken cancellationToken)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            HangUpImpl(connection.Handle, closeAllReferences, cancellationToken);
            return Task.CompletedTask;
        }

        public void UnsafeHangUp(IntPtr handle, bool closeAllReferences, CancellationToken cancellationToken)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            HangUpImpl(handle, closeAllReferences, cancellationToken);
        }

        private void HangUpImpl(IntPtr handle, bool closeAllReferences, CancellationToken cancellationToken)
        {
            CloseAllReferencesToTheHandle(handle, closeAllReferences, cancellationToken);
            EnsurePortHasBeenReleased();
        }

        private void CloseAllReferencesToTheHandle(IntPtr handle, bool closeAllReferences, CancellationToken cancellationToken)
        {
            int ret;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                ret = api.RasHangUp(handle);
                if (ShouldThrowExceptionFromReturnCode(ret))
                {
                    throw exceptionPolicy.Create(ret);
                }
            } while (closeAllReferences && ret == SUCCESS);
        }

        private static bool ShouldThrowExceptionFromReturnCode(int ret)
        {
            return ret != SUCCESS && ret != ERROR_NO_CONNECTION;
        }

        private static void EnsurePortHasBeenReleased()
        {
            // ATTENTION! This required pause comes from the Windows SDK. Failure to perform this pause may cause the state machine to leave 
            // the port open which will require the machine to be rebooted to release the port.
            Thread.Sleep(1000);
        }
    }
}