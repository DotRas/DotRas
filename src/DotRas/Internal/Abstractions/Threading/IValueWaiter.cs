using System.Threading;

namespace DotRas.Internal.Abstractions.Threading
{
    internal interface IValueWaiter<T>
    {
        T Value { get; }
        bool IsSet { get; }

        void Reset();
        void WaitForValue(CancellationToken cancellationToken);
        void Set(T value);
    }
}