using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    public interface ILog
    {
        void Event(EventLevel eventLevel, TraceEvent eventData);
    }
}