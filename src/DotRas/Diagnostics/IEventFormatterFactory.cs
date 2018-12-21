using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies a factory of event formatters.
    /// </summary>
    public interface IEventFormatterFactory
    {
        /// <summary>
        /// Creates a formatter for the type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="TraceEvent"/> to be formatted.</typeparam>
        /// <returns>The formatter instance.</returns>
        IEventFormatter<T> Create<T>() where T : TraceEvent;
    }
}