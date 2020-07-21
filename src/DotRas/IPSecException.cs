using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when an IPSec related error code occurs within the operating system.
    /// </summary>
    [Serializable]
    public class IPSecException : Win32Exception
    {
        /// <summary>
        /// Initializes an instance of the <see cref="IPSecException"/> class.
        /// </summary>
        public IPSecException()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="IPSecException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public IPSecException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="IPSecException"/> class.
        /// </summary>
        /// <param name="error">The Win32 error code.</param>
        public IPSecException(int error)
            : base(error)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="IPSecException"/> class.
        /// </summary>
        /// <param name="error">The Win32 error code.</param>
        /// <param name="message">A message describing the error.</param>
        public IPSecException(int error, string message)
            : base(error, message)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="IPSecException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public IPSecException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected IPSecException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}