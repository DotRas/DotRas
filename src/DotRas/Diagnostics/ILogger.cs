using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies a mechanism which can log events that occur.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="eventLevel">The level of the event which occurred.</param>
        /// <param name="eventData">An object containing event data.</param>
        void Log(EventLevel eventLevel, TraceEvent eventData);
    }
}