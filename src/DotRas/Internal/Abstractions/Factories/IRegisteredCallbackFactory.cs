using System.Threading;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IRegisteredCallbackFactory
    {
        IRegisteredCallback Create(WaitOrTimerCallback callback, object state);
    }
}