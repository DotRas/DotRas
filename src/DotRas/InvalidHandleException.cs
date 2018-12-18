using System;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when an invalid handle has been used to perform an operation.
    /// </summary>
    [Serializable]
    public class InvalidHandleException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHandleException"/> class.
        /// </summary>
        public InvalidHandleException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHandleException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public InvalidHandleException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHandleException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public InvalidHandleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected InvalidHandleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}