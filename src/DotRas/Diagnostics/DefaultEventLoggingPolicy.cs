using System;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics
{
    internal class DefaultEventLoggingPolicy : IEventLoggingPolicy
    {
        private readonly ILogger logger;

        public DefaultEventLoggingPolicy(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogEvent(EventLevel eventLevel, TraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            try
            {
                logger.Log(eventLevel, eventData);
            }
            catch (Exception)
            {
                // Swallow any exceptions which occur while attempting to log the event.
            }
        }
    }
}