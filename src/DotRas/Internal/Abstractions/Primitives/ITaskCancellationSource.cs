using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface ITaskCancellationSource : IDisposable
    {
        CancellationToken Token { get; }

        void Cancel();
        void CancelAfter(TimeSpan delay);
    }
}