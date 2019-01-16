using System;
using System.Threading;
using DotRas.Internal.Abstractions.Primitives;
using Microsoft.Win32.SafeHandles;

namespace DotRas.Internal.Infrastructure.Primitives
{
    internal class RegisteredCallback : DisposableObject, IRegisteredCallback
    {
        private readonly AutoResetEvent waitEvent;
        private readonly RegisteredWaitHandle waitHandle;

        public SafeWaitHandle Handle => waitEvent.SafeWaitHandle;

        private RegisteredCallback(AutoResetEvent waitEvent, RegisteredWaitHandle waitHandle)
        {
            this.waitEvent = waitEvent ?? throw new ArgumentNullException(nameof(waitEvent));
            this.waitHandle = waitHandle ?? throw new ArgumentNullException(nameof(waitHandle));
        }

        public static IRegisteredCallback Create(WaitOrTimerCallback callback, object state)
        {
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

        protected override void Dispose(bool disposing)
        {
            waitHandle.Unregister(waitEvent);
            waitEvent.Dispose();

            base.Dispose(disposing);
        }
    }
}