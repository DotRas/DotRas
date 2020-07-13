using System;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when the application is not using a product version compatible with the version of Windows upon which the application is executing.
    /// </summary>
    [Serializable]
    public class OperatingSystemNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSystemNotSupportedException"/> class.
        /// </summary>
        public OperatingSystemNotSupportedException() 
            : this("The operating system does not support the operation being requested. Please check the compatibility matrix for features supported with this operating system.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSystemNotSupportedException"/> class.
        /// </summary>
        /// <param name="message">The error message which describes the reason for the error.</param>
        public OperatingSystemNotSupportedException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSystemNotSupportedException"/> class.
        /// </summary>
        /// <param name="message">The error message which describes the reason for the error.</param>
        /// <param name="innerException">The exception which was the cause of this exception.</param>
        public OperatingSystemNotSupportedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSystemNotSupportedException"/> class.
        /// </summary>
        protected OperatingSystemNotSupportedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}