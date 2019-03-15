using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Internal.Infrastructure.Advice
{
    internal abstract class LoggingAdvice<T> : DisposableObject
        where T : class
    {
        private readonly IEventLoggingPolicy eventLoggingPolicy;

        protected LoggingAdvice(T attachedObject, IEventLoggingPolicy eventLoggingPolicy)
        {
            this.AttachedObject = attachedObject ?? throw new ArgumentNullException(nameof(attachedObject));
            this.eventLoggingPolicy = eventLoggingPolicy ?? throw new ArgumentNullException(nameof(eventLoggingPolicy));
        }

        protected T AttachedObject { get; }

        protected void LogInformation(TraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            eventLoggingPolicy.LogEvent(EventLevel.Information, eventData);
        }

        protected void LogVerbose(TraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            eventLoggingPolicy.LogEvent(EventLevel.Verbose, eventData);
        }
    }
}