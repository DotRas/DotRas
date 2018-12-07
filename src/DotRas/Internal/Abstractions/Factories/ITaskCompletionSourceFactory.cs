using DotRas.Internal.Abstractions.Threading;

namespace DotRas.Internal.Abstractions.Factories
{
    internal interface ITaskCompletionSourceFactory
    {
        ITaskCompletionSource<T> Create<T>();
    }
}