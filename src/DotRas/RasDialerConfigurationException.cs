using System;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when a configuration issue has been found while dialing a connection using the <see cref="RasDialer"/>.
    /// </summary>
    [Serializable]
    public class RasDialerConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RasDialerConfigurationException"/> class.
        /// </summary>
        public RasDialerConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasDialerConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        public RasDialerConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasDialerConfigurationException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public RasDialerConfigurationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected RasDialerConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}