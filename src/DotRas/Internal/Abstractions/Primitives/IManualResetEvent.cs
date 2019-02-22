using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface IManualResetEvent : IDisposable
    {
        void Set();
        void Reset();

        void Wait(CancellationToken cancellationToken);
    }
}