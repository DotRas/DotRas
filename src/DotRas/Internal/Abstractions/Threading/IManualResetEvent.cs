using System.Threading;

namespace DotRas.Internal.Abstractions.Threading
{
    internal interface IManualResetEvent
    {
        void Set();
        void Reset();

        void Wait(CancellationToken cancellationToken);
    }
}