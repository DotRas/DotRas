using System.Threading;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Infrastructure.Primitives
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