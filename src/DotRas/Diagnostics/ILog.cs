using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    public interface ILog
    {
        void HandleEvent(EventLevel eventLevel, TraceEvent eventData);
    }
}