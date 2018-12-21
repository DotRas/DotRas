using System;
using DotRas.Diagnostics.Tracing;
using DotRas.Diagnostics.Tracing.Formatters;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    [Formatter(typeof(PInvokeInt32CallCompletedTraceEventFormatter))]
    public class PInvokeInt32CallCompletedTraceEvent : PInvokeCallCompletedTraceEvent<int?>
    {
    }
}