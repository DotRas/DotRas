using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using Microsoft.Extensions.Logging;

namespace ConsoleRunner.Infrastructure.Diagnostics
{
    class DotRasLoggingAdapter : DotRas.Diagnostics.ILogger
    {
        private readonly IEventFormatterAdapter adapter;
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public DotRasLoggingAdapter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("DotRas");
            adapter = new EventFormatterAdapter(new ConventionBasedEventFormatterFactory());
        }

        public void Log(EventLevel eventLevel, TraceEvent eventData)
        {
            if (eventData == null)
            {
                return;
            }

            logger.Log(ConvertToLogLevel(eventLevel), FormatEventData(eventData));
        }

        private string FormatEventData(TraceEvent eventData)
        {
            return adapter.Format(eventData);
        }

        private static LogLevel ConvertToLogLevel(EventLevel level)
        {
            switch (level)
            {
                case EventLevel.Critical:
                    return LogLevel.Critical;

                case EventLevel.Error:
                    return LogLevel.Error;

                case EventLevel.Warning:
                    return LogLevel.Warning;

                case EventLevel.Information:
                    return LogLevel.Information;

                default:
                    return LogLevel.Debug;
            }
        }
    }
}