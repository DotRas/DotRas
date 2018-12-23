using System.Diagnostics;

namespace DotRas.Diagnostics.Tracing
{
    internal interface IEventLevelConverter
    {
        TraceEventType Convert(EventLevel input);
    }
}