using DotRas.Diagnostics.Events;
using System;
using System.Reflection;

namespace DotRas.Diagnostics {
    /// <summary>
    /// Provides a generic adapter for formatting trace events.
    /// </summary>
    public class EventFormatterAdapter : IEventFormatterAdapter {
        private const string FormatMethodName = nameof(IEventFormatter<TraceEvent>.Format);
        private const string FactoryMethodName = nameof(IEventFormatterFactory.Create);

        private readonly IEventFormatterFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFormatterAdapter"/> class.
        /// </summary>
        /// <param name="factory">The factory to use to create the formatter.</param>
        public EventFormatterAdapter(IEventFormatterFactory factory) {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc />
        public string Format(object eventData) {
            if (eventData == null) {
                return "[(null)]";
            }

            var formatter = CreateFormatterForType(eventData.GetType());
            return formatter == null ? throw new FormatterNotFoundException($"The formatter for event data type '{eventData.GetType()}' could not be located.") : FormatValue(formatter, eventData);
        }

        private object CreateFormatterForType(Type valueType) {
            var factoryType = factory.GetType();

            var factoryMethod = factoryType.GetMethod(FactoryMethodName, BindingFlags.Instance | BindingFlags.Public)?.MakeGenericMethod(valueType);
            return factoryMethod == null ? throw new InvalidOperationException("The factory method could not be located.") : factoryMethod.Invoke(factory, null);
        }

        private static string FormatValue(object formatter, object value) {
            var formatterType = formatter.GetType();

            var method = formatterType.GetMethod(FormatMethodName, BindingFlags.Instance | BindingFlags.Public);
            return method == null ? throw new MissingMethodException(formatterType.Name, FormatMethodName) : (string)method.Invoke(formatter, new[] { value });
        }
    }
}
