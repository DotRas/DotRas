using System;
using System.Threading;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Primitives
{
    internal class ValueWaiter<T> : DisposableObject, IValueWaiter<T>
    {
        private readonly object syncRoot = new object();
        private readonly IManualResetEvent waitHandle;

        public ValueWaiter(IManualResetEvent waitHandle)
        {
            this.waitHandle = waitHandle ?? throw new ArgumentNullException(nameof(waitHandle));
        }

        public T Value { get; private set; }
        public bool IsSet { get; private set; }

        public void Reset()
        {
            lock (syncRoot)
            {
                Value = default;
                IsSet = false;
            }

            waitHandle.Reset();
        }

        public void WaitForValue(CancellationToken cancellationToken)
        {
            waitHandle.Wait(cancellationToken);
        }

        public void Set(T value)
        {
            lock (syncRoot)
            {
                Value = value;
                IsSet = true;
            }

            waitHandle.Set();        
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                waitHandle.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}