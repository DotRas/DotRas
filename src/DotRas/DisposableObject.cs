using System;

namespace DotRas
{
    /// <summary>
    /// Provides a base class for an object which is disposable. This class must be inherited.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Gets an object used for thread synchronization.
        /// </summary>
        protected object SyncRoot { get; } = new object();

        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            lock (SyncRoot)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or releasing unmanaged resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources, false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                disposed = true;
            }
        }

        protected void GuardMustNotBeDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}