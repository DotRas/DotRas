using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when a Win32 error occurs within the Remote Access Service (RAS) client within the operating system.
    /// </summary>
    [Serializable]
    public class RasException : Win32Exception
    {
        /// <summary>
        /// Initializes an instance of the <see cref="RasException"/> class.
        /// </summary>
        public RasException()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RasException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public RasException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RasException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public RasException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RasException"/> class.
        /// </summary>
        /// <param name="error">The Win32 error code.</param>
        /// <param name="message">A message describing the error.</param>
        public RasException(int error, string message)
            : base(error, message)
        {
        }

        /// <inheritdoc />
        protected RasException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}