using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Primitives;

namespace DotRas.Internal.Composition.Factories
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