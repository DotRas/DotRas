using System;
using System.Threading;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Infrastructure.Primitives;

namespace DotRas.Internal.Infrastructure.Factories
{
    internal class RegisteredCallbackFactory : IRegisteredCallbackFactory
    {
        public IRegisteredCallback Create(WaitOrTimerCallback callback, object state)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            AutoResetEvent waitEvent = null;

            try
            {
                waitEvent = new AutoResetEvent(false);
                RegisteredWaitHandle waitHandle = null;

                try
                {
                    waitHandle = ThreadPool.RegisterWaitForSingleObject(waitEvent, callback, state, Timeout.Infinite, false);

                    return new RegisteredCallback(
                        waitEvent,
                        waitHandle);
                }
                catch (Exception)
                {
                    waitHandle?.Unregister(waitEvent);
                    throw;
                }
            }
            catch (Exception)
            {
                waitEvent?.Dispose();
                throw;
            }
        }
    }
}