using System;
using System.Runtime.Serialization;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Thrown when an exception occurs while attempting to find the formatter for an event.
    /// </summary>
    [Serializable]
    public class FormatterNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterNotFoundException"/> class.
        /// </summary>
        public FormatterNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterNotFoundException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public FormatterNotFoundException(string message) 
            : base(message)
        {
        }  

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterNotFoundException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public FormatterNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected FormatterNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}