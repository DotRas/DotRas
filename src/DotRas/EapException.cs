using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when an exception occurs within the Extensible Authentication Protocol (EAP) client within the operating system.
    /// </summary>
    [Serializable]
    public class EapException : Win32Exception
    {
        /// <summary>
        /// Initializes an instance of the <see cref="EapException"/> class.
        /// </summary>
        public EapException()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="EapException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public EapException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="EapException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public EapException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="EapException"/> class.
        /// </summary>
        /// <param name="error">The Win32 error code.</param>
        /// <param name="message">A message describing the error.</param>
        public EapException(int error, string message)
            : base(error, message)
        {
        }

        /// <inheritdoc />
        protected EapException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}