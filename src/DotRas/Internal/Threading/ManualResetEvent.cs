using System.Threading;
using DotRas.Internal.Abstractions.Threading;

namespace DotRas.Internal.Threading
{
    internal class ManualResetEvent : DisposableObject, IManualResetEvent
    {
        private readonly ManualResetEventSlim waitHandle = new ManualResetEventSlim();

        public void Set()
        {
            GuardMustNotBeDisposed();

            waitHandle.Set();
        }

        public void Reset()
        {
            GuardMustNotBeDisposed();

            waitHandle.Reset();
        }

        public void Wait(CancellationToken cancellationToken)
        {
            GuardMustNotBeDisposed();

            if (!waitHandle.IsSet)
            {
                waitHandle.Wait(cancellationToken);
            }
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