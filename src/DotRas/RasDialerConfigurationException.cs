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
        /// <param name="message">The message describing the error.</param>
        public RasDialerConfigurationException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        protected RasDialerConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}