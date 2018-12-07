using System;
using System.Threading;
using DotRas.Internal.Abstractions.Threading;

namespace DotRas.Internal.Threading
{
    internal class TaskCancellationSource : DisposableObject, ITaskCancellationSource
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        public TaskCancellationSource(CancellationTokenSource cancellationTokenSource)
        {
            this.cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public CancellationToken Token => cancellationTokenSource.Token;

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        public void CancelAfter(TimeSpan delay)
        {
            cancellationTokenSource.CancelAfter(delay);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancellationTokenSource.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}