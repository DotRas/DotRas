using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface ITaskCancellationSource
    {
        CancellationToken Token { get; }

        void Cancel();
        void CancelAfter(TimeSpan delay);
    }
}