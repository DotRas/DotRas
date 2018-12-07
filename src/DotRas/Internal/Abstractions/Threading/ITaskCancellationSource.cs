using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Threading
{
    internal interface ITaskCancellationSource
    {
        CancellationToken Token { get; }

        void Cancel();
        void CancelAfter(TimeSpan delay);
    }
}