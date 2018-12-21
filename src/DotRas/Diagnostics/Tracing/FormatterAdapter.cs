using System;
using System.Reflection;

namespace DotRas.Diagnostics.Tracing
{
    internal class FormatterAdapter : IFormatterAdapter
    {
        private const string FormatMethodName = nameof(IFormatter<object>.Format);
        private const string FactoryMethodName = nameof(IFormatterFactory.Create);

        private readonly IFormatterFactory factory;

        public FormatterAdapter(IFormatterFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public string Format(object value)
        {
            if (value == null)
            {
                return "[(null)]";
            }

            var formatter = CreateFormatterForType(value.GetType());
            if (formatter == null)
            {
                throw new FormatterNotFoundException($"The formatter could not be located.", value.GetType());
            }

            return FormatValue(formatter, value);            
        }

        private object CreateFormatterForType(Type valueType)
        {
            var factoryType = factory.GetType();

            var factoryMethod = factoryType.GetMethod(FactoryMethodName, BindingFlags.Instance | BindingFlags.Public)?.MakeGenericMethod(valueType);
            if (factoryMethod == null)
            {
                throw new InvalidOperationException("The factory method could not be located.");
            }

            return factoryMethod.Invoke(factory, null);
        }

        private static string FormatValue(object formatter, object value)
        {
            var formatterType = formatter.GetType();

            var method = formatterType.GetMethod(FormatMethodName, BindingFlags.Instance | BindingFlags.Public);
            if (method == null)
            {
                throw new MissingMethodException(formatterType.Name, FormatMethodName);
            }

            return (string)method.Invoke(formatter, new[] { value });
        }
    }
}