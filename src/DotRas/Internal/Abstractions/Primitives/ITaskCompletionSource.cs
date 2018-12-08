using System;
using System.Threading.Tasks;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface ITaskCompletionSource<T>
    {
        Task<T> Task { get; }

        void SetResultAsynchronously(T result);
        void SetResult(T result);

        void SetExceptionAsynchronously(Exception exception);
        void SetException(Exception exception);
    }
}