using System.Threading;
using DotRas.Internal.Abstractions.Threading;

namespace DotRas.Internal.Abstractions.Factories
{
    internal interface ITaskCancellationSourceFactory
    {
        ITaskCancellationSource Create();

        ITaskCancellationSource Create(CancellationToken linkedCancellationToken);
    }
}