using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasHangUp
    {
        void HangUp(IRasConnection connection, CancellationToken cancellationToken);

        void HangUp(IntPtr handle, CancellationToken cancellationToken);
    }
}