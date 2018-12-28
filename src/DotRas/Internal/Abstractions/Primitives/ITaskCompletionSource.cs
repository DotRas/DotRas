using System;
using System.Threading.Tasks;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface ITaskCompletionSource<T>
    {
        Task<T> Task { get; }

        void SetResult(T result);
        void SetException(Exception exception);
    }
}