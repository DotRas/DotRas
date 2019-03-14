namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies a generic adapter for formatting events.
    /// </summary>
    public interface IEventFormatterAdapter
    {
        /// <summary>
        /// Formats the event data to a string.
        /// </summary>
        /// <param name="eventData">The event data to format.</param>
        /// <returns>The formatted event data.</returns>
        string Format(object eventData);
    }
}