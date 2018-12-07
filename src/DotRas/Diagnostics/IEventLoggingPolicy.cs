using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    internal interface IEventLoggingPolicy
    {
        void LogEvent(EventLevel eventLevel, TraceEvent eventData);
    }
}