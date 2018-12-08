using System.Threading;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface IManualResetEvent
    {
        void Set();
        void Reset();

        void Wait(CancellationToken cancellationToken);
    }
}