using System;
using DotRas.Diagnostics.Tracing;
using DotRas.Diagnostics.Tracing.Formatters;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    [Formatter(typeof(RasDialCallbackCompletedTraceEventFormatter))]
    public class RasDialCallbackCompletedTraceEvent : CallbackTraceEvent
    {
        public bool Result { get; set; }
    }
}