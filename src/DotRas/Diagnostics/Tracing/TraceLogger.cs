using System;
using System.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Tracing
{
    internal class TraceLogger : ILogger
    {
        private readonly TraceSource source = new TraceSource("DotRas", SourceLevels.Off);

        private readonly IEventFormatterAdapter formatter;
        private readonly IEventLevelConverter eventLevelConverter;

        public TraceLogger(IEventFormatterAdapter formatter, IEventLevelConverter eventLevelConverter)
        {
            this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            this.eventLevelConverter = eventLevelConverter ?? throw new ArgumentNullException(nameof(eventLevelConverter));
        }

        public void Log(EventLevel eventLevel, TraceEvent eventData)
        {
            lock (source)
            {
                source.TraceEvent(
                    eventLevelConverter.Convert(eventLevel),
                    0,
                    formatter.Format(eventData));
            }
        }
    }
}