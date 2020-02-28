using System;
using System.Diagnostics;

namespace DotRas.Diagnostics.Tracing
{
    internal class EventLevelConverter : IEventLevelConverter
    {
        public TraceEventType Convert(EventLevel input)
        {
            return input switch
            {
                EventLevel.Critical => TraceEventType.Critical,
                EventLevel.Error => TraceEventType.Error,
                EventLevel.Information => TraceEventType.Information,
                EventLevel.Warning => TraceEventType.Warning,
                EventLevel.Verbose => TraceEventType.Verbose,
                _ => throw new NotSupportedException($"The value '{input}' is not supported.")
            };
        }
    }
}