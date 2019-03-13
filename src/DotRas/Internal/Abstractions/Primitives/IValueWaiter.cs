using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface IValueWaiter<T> : IDisposable
    {
        T Value { get; }
        bool IsSet { get; }

        void Reset();
        void WaitForValue(CancellationToken cancellationToken);
        void Set(T value);
    }
}