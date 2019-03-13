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

        public RegisteredCallback(AutoResetEvent waitEvent, RegisteredWaitHandle waitHandle)
        {
            this.waitEvent = waitEvent ?? throw new ArgumentNullException(nameof(waitEvent));
            this.waitHandle = waitHandle ?? throw new ArgumentNullException(nameof(waitHandle));
        }        

        protected override void Dispose(bool disposing)
        {
            waitHandle.Unregister(waitEvent);
            waitEvent.Dispose();

            base.Dispose(disposing);
        }
    }
}