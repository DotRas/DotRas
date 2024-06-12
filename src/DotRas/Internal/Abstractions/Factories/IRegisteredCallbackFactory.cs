using DotRas.Internal.Abstractions.Primitives;
using System.Threading;

namespace DotRas.Internal.Abstractions.Factories {
    internal interface IRegisteredCallbackFactory {
        IRegisteredCallback Create(WaitOrTimerCallback callback, object state);
    }
}
