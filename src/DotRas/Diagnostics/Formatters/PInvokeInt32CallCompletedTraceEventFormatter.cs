using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for an <see cref="PInvokeInt32CallCompletedTraceEvent"/> event.
    /// </summary>
    public class PInvokeInt32CallCompletedTraceEventFormatter : 
        PInvokeCallCompletedTraceEventFormatter<PInvokeInt32CallCompletedTraceEvent, int?>
    {
    }
}