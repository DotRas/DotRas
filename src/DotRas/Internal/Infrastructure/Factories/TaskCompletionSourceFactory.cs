using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Infrastructure.Primitives;

namespace DotRas.Internal.Infrastructure.Factories
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