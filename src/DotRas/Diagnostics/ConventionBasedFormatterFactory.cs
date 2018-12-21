using System;
using System.Reflection;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Provides a factory which uses an attribute-based convention for identifying the factory for a type.
    /// </summary>
    public class ConventionBasedFormatterFactory : IFormatterFactory
    {
        /// <summary>
        /// Creates a formatter.
        /// </summary>
        /// <typeparam name="T">The type of object which needs to be formatted.</typeparam>
        /// <returns>The formatter instance.</returns>
        /// <exception cref="FormatterNotFoundException">The formatter for type <typeparamref name="T"/> could not be determined based on the convention.</exception>
        public IFormatter<T> Create<T>()
        {
            var valueType = typeof(T);

            var attribute = valueType.GetCustomAttribute<FormatterAttribute>();
            if (attribute == null)
            {
                throw new FormatterNotFoundException("The formatter could not be identified.", valueType);
            }

            GuardTheFormatterIsTheCorrectType(valueType, attribute.FormatterType);

            try
            {
                return (IFormatter<T>)Activator.CreateInstance(attribute.FormatterType);
            }
            catch (MissingMethodException)
            {
                throw new FormatterNotFoundException("The formatter could not be created. Please verify the formatter contains a parameterless constructor.", valueType, attribute.FormatterType);
            }
        }

        private static void GuardTheFormatterIsTheCorrectType(Type valueType, Type formatterType)
        {
            var expected = typeof(IFormatter<>).MakeGenericType(valueType);
            if (!expected.IsAssignableFrom(formatterType))
            {
                throw new InvalidOperationException("The formatter does not implement the correct interface.");
            }
        }
    }
}