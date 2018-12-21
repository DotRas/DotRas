using System;
using DotRas.Diagnostics.Formatters;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes a platform invocation call completion event which uses a boolean based result.
    /// </summary>
    [Serializable]
    [EventFormatter(typeof(PInvokeBoolCallCompletedTraceEventFormatter))]
    public class PInvokeBoolCallCompletedTraceEvent : PInvokeCallCompletedTraceEvent<bool>
    {
    }
}