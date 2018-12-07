using System.Threading;
using System.Threading.Tasks;
using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasHangUp
    {
        Task HangUpAsync(RasHandle handle, CancellationToken cancellationToken);
        void HangUp(RasHandle handle, CancellationToken cancellationToken);
    }
}