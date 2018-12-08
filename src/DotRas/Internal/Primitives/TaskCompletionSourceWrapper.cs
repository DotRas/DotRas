using System;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Primitives
{
    internal class TaskCompletionSourceWrapper<T> : ITaskCompletionSource<T>
    {
        private readonly TaskCompletionSource<T> completionSource;

        public TaskCompletionSourceWrapper(TaskCompletionSource<T> completionSource)
        {
            this.completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
        }

        public Task<T> Task => completionSource.Task;

        public void SetResultAsynchronously(T result)
        {
            System.Threading.Tasks.Task.Run(() => SetResult(result));
        }

        public void SetResult(T result)
        {
            completionSource.SetResult(result);
        }

        public void SetExceptionAsynchronously(Exception exception)
        {
            System.Threading.Tasks.Task.Run(() => SetException(exception));
        }

        public void SetException(Exception exception)
        {
            completionSource.SetException(exception);
        }
    }
}