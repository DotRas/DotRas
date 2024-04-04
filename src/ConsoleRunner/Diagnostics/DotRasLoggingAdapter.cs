using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using NLog;

namespace ConsoleRunner.Diagnostics;

class DotRasLoggingAdapter : DotRas.Diagnostics.ILogger
{
    private readonly IEventFormatterAdapter adapter;
    private readonly NLog.ILogger logger;

    public DotRasLoggingAdapter()
    {
        logger = LogManager.GetLogger("App");
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
                return LogLevel.Fatal;

            case EventLevel.Error:
                return LogLevel.Error;

            case EventLevel.Warning:
                return LogLevel.Warn;

            case EventLevel.Information:
                return LogLevel.Info;

            default:
                return LogLevel.Debug;
        }
    }
}