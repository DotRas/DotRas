using System;
using System.Threading;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Infrastructure.Primitives
{
    internal class RegisteredCallback : DisposableObject, IRegisteredCallback
    {
        private readonly AutoResetEvent waitEvent;
        private readonly RegisteredWaitHandle waitHandle;
        
        public ISafeHandleWrapper Handle { get; }

        public RegisteredCallback(AutoResetEvent waitEvent, RegisteredWaitHandle waitHandle)
        {
            this.waitEvent = waitEvent ?? throw new ArgumentNullException(nameof(waitEvent));
            this.waitHandle = waitHandle ?? throw new ArgumentNullException(nameof(waitHandle));

            Handle = new SafeHandleWrapper(this.waitEvent.SafeWaitHandle);
        }        

        protected override void Dispose(bool disposing)
        {
            waitHandle.Unregister(waitEvent);
            waitEvent.Dispose();

            base.Dispose(disposing);
        }
    }
}