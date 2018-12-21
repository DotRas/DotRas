using System;
using DotRas.Diagnostics.Tracing;
using DotRas.Diagnostics.Tracing.Formatters;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    [Formatter(typeof(PInvokeBoolCallCompletedTraceEventFormatter))]
    public class PInvokeBoolCallCompletedTraceEvent : PInvokeCallCompletedTraceEvent<bool>
    {
    }
}