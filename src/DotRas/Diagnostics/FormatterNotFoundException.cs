using System;
using System.Runtime.Serialization;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Thrown when an exception occurs while attempting to find the correct formatter for a particular type of event.
    /// </summary>
    [Serializable]
    public class FormatterNotFoundException : Exception
    {
        /// <summary>
        /// Gets the target type of the formatter.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets the type of formatter.
        /// </summary>
        public Type FormatterType { get; }

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
        /// <param name="targetType">The target type of the formatter.</param>
        /// <param name="formatterType">Optional. The type of formatter used by the target type.</param>
        public FormatterNotFoundException(string message, Type targetType, Type formatterType = null)
            : base(message)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            FormatterType = formatterType;
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