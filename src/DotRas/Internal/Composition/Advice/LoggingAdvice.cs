using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Internal.Composition.Advice
{
    internal abstract class LoggingAdvice<T>
        where T : class
    {
        private readonly T attachedObject;
        private readonly IEventLoggingPolicy eventLoggingPolicy;

        protected LoggingAdvice(T attachedObject, IEventLoggingPolicy eventLoggingPolicy)
        {
            this.attachedObject = attachedObject ?? throw new ArgumentNullException(nameof(attachedObject));
            this.eventLoggingPolicy = eventLoggingPolicy ?? throw new ArgumentNullException(nameof(eventLoggingPolicy));
        }

        protected T AttachedObject
        {
            get { return attachedObject; }
        }

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