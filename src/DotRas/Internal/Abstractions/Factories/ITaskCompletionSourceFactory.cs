using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Abstractions.Factories
{
    internal interface ITaskCompletionSourceFactory
    {
        ITaskCompletionSource<T> Create<T>();
    }
}