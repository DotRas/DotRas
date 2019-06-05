using System;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Primitives;
using static System.Threading.Tasks.Task;

namespace DotRas.Internal.Infrastructure.Primitives
{
    internal class TaskCompletionSourceWrapper<T> : ITaskCompletionSource<T>
    {
        private readonly TaskCompletionSource<T> completionSource;

        public TaskCompletionSourceWrapper(TaskCompletionSource<T> completionSource)
        {
            this.completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
        }

        public Task<T> Task => completionSource.Task;

        public void SetResult(T result)
        {
            Run(() => completionSource.SetResult(result));
        }

        public void SetException(Exception exception)
        {
            Run(() => completionSource.SetException(exception));        
        }
    }
}