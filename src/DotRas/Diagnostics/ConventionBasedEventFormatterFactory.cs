using System;
using System.Reflection;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Provides a factory which uses an attribute-based convention for identifying the factory for a type.
    /// </summary>
    public class ConventionBasedEventFormatterFactory : IEventFormatterFactory
    {
        /// <summary>
        /// Creates a formatter.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="TraceEvent"/> to be formatted.</typeparam>
        /// <returns>The formatter instance.</returns>
        /// <exception cref="FormatterNotFoundException">The formatter for type <typeparamref name="T"/> could not be determined based on the convention.</exception>
        public IEventFormatter<T> Create<T>() 
            where T : TraceEvent
        {
            var valueType = typeof(T);

            var attribute = valueType.GetCustomAttribute<EventFormatterAttribute>();
            if (attribute == null)
            {
                throw new FormatterNotFoundException("The formatter could not be identified.", valueType);
            }

            GuardTheFormatterIsTheCorrectType(valueType, attribute.FormatterType);

            try
            {
                return (IEventFormatter<T>)Activator.CreateInstance(attribute.FormatterType);
            }
            catch (MissingMethodException)
            {
                throw new FormatterNotFoundException("The formatter could not be created. Please verify the formatter contains a parameterless constructor.", valueType, attribute.FormatterType);
            }
        }

        private static void GuardTheFormatterIsTheCorrectType(Type valueType, Type formatterType)
        {
            var expected = typeof(IEventFormatter<>).MakeGenericType(valueType);
            if (!expected.IsAssignableFrom(formatterType))
            {
                throw new InvalidOperationException("The formatter does not implement the correct interface.");
            }
        }
    }
}