using System;
using System.Reflection;

namespace DotRas.Diagnostics.Tracing
{
    internal class FormatterAdapter : IFormatterAdapter
    {
        private const string FormatMethodName = nameof(IFormatter<object>.Format);

        public string Format(object value)
        {
            if (value == null)
            {
                return "[(null)]";
            }

            var valueType = value.GetType();

            var attribute = valueType.GetCustomAttribute<FormatterAttribute>();
            if (attribute == null)
            {
                throw new FormatterNotFoundException($"The formatter for type '{valueType.FullName} could not be identified.");
            }

            var expected = typeof(IFormatter<>).MakeGenericType(valueType);
            if (!expected.IsAssignableFrom(attribute.FormatterType))
            {
                throw new InvalidOperationException($"The formatter does not implement the correct interface. Expected: {expected.FullName}");
            }

            var formatter = Activator.CreateInstance(attribute.FormatterType);
            if (formatter == null)
            {
                throw new FormatterNotFoundException($"The formatter '{attribute.FormatterType.FullName}' could not be created.");
            }

            var method = attribute.FormatterType.GetMethod(FormatMethodName, BindingFlags.Instance | BindingFlags.Public);
            if (method == null)
            {
                throw new MissingMethodException(attribute.FormatterType.Name, FormatMethodName);
            }

            return (string)method.Invoke(formatter, new[] { value });
        }
    }
}