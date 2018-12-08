using System.Threading;
using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasHangUp
    {
        void HangUp(RasHandle handle, CancellationToken cancellationToken);
    }
}