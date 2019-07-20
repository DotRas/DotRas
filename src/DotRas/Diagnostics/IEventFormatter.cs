using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies a formatter for a trace event.
    /// </summary>
    /// <typeparam name="T">The type of event being formatted.</typeparam>
    public interface IEventFormatter<in T>
        where T : TraceEvent
    {
        /// <summary>
        /// Formats the event data.
        /// </summary>
        /// <param name="eventData">The event data to format.</param>
        /// <returns>The formatted value.</returns>
        string Format(T eventData);
    }
}