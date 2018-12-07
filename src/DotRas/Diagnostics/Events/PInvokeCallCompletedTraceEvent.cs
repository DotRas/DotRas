using System;
using DotRas.Diagnostics.Tracing;
using DotRas.Diagnostics.Tracing.Formatters;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    [Formatter(typeof(PInvokeCallCompletedTraceEventFormatter))]
    public class PInvokeCallCompletedTraceEvent : CallCompletedTraceEvent
    {
        public int? Result { get; set; }
        public string DllName { get; set; }
        public string MethodName { get; set; }
    }
}