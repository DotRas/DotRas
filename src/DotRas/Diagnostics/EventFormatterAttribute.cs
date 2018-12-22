using System;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies the event formatter for a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public sealed class EventFormatterAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of formatter.
        /// </summary>
        public Type FormatterType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFormatterAttribute"/> class.
        /// </summary>
        /// <param name="formatterType">The type of formatter.</param>
        public EventFormatterAttribute(Type formatterType)
        {
            FormatterType = formatterType ?? throw new ArgumentNullException(nameof(formatterType));
        }
    }
}