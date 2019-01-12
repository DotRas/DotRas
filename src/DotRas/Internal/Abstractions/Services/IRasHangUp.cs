using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasHangUp
    {
        void HangUp(IRasConnection connection, bool closeAllReferences, CancellationToken cancellationToken);

        void UnsafeHangUp(IntPtr handle, bool closeAllReferences);
    }
}