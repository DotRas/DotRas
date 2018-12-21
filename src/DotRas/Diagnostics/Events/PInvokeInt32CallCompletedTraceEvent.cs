using System;
using DotRas.Diagnostics.Formatters;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes a platform invocation call completion event which uses an integer based result.
    /// </summary>
    [Serializable]
    [Formatter(typeof(PInvokeInt32CallCompletedTraceEventFormatter))]
    public class PInvokeInt32CallCompletedTraceEvent : PInvokeCallCompletedTraceEvent<int?>
    {
    }
}