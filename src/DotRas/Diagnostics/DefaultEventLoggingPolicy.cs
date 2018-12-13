using System;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    internal class DefaultEventLoggingPolicy : IEventLoggingPolicy
    {
        private readonly ILog log;

        public DefaultEventLoggingPolicy(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void LogEvent(EventLevel eventLevel, TraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            try
            {
                log.HandleEvent(eventLevel, eventData);
            }
            catch (Exception)
            {
                // Swallow any exceptions which occur while attempting to log the event.
            }
        }
    }
}