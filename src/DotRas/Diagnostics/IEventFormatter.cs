using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies a formatter for an object.
    /// </summary>
    /// <typeparam name="T">The type of object being formatted.</typeparam>
    public interface IEventFormatter<in T>
        where T : TraceEvent
    {
        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted value.</returns>
        string Format(T value);
    }
}