using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for an <see cref="PInvokeBoolCallCompletedTraceEvent"/> event.
    /// </summary>
    public class PInvokeBoolCallCompletedTraceEventFormatter : 
        PInvokeCallCompletedTraceEventFormatter<PInvokeBoolCallCompletedTraceEvent, bool>
    {
    }
}