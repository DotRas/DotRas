using System;

namespace DotRas
{
    /// <summary>
    /// Contains event data for error events.
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        private readonly Exception error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="error">The exception which occurred.</param>
        public ErrorEventArgs(Exception error)
        {
            this.error = error ?? throw new ArgumentNullException(nameof(error));
        }

        /// <summary>
        /// Gets the exception which occurred.
        /// </summary>
        /// <returns>The exception which occurred.</returns>
        public Exception GetException()
        {
            return error;
        }
    }
}