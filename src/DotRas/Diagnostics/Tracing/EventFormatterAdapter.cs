using System;
using System.Reflection;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Tracing
{
    internal class EventFormatterAdapter : IEventFormatterAdapter
    {
        private const string FormatMethodName = nameof(IEventFormatter<TraceEvent>.Format);
        private const string FactoryMethodName = nameof(IEventFormatterFactory.Create);

        private readonly IEventFormatterFactory factory;

        public EventFormatterAdapter(IEventFormatterFactory factory)
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