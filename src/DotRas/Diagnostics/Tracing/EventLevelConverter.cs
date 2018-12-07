using System;
using System.Diagnostics;

namespace DotRas.Diagnostics.Tracing
{
    internal class EventLevelConverter : IConverter<EventLevel, TraceEventType>
    {
        public TraceEventType Convert(EventLevel input)
        {
            switch (input)
            {
                case EventLevel.Critical:
                    return TraceEventType.Critical;

                case EventLevel.Error:
                    return TraceEventType.Error;

                case EventLevel.Information:
                    return TraceEventType.Information;

                case EventLevel.Warning:
                    return TraceEventType.Warning;

                case EventLevel.Verbose:
                    return TraceEventType.Verbose;

                default:
                    throw new NotSupportedException($"The value '{input}' is not supported.");
            }
        }
    }
}