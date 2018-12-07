using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Internal.Threading;

namespace DotRas.Internal.Factories
{
    internal class TaskCompletionSourceFactory : ITaskCompletionSourceFactory
    {
        public ITaskCompletionSource<T> Create<T>()
        {
            return new TaskCompletionSourceWrapper<T>(
                new TaskCompletionSource<T>());
        }
    }
}