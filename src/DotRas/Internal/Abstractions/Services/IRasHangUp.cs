using System.Threading;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasHangUp
    {
        void HangUp(RasHandle handle, CancellationToken cancellationToken);
    }
}