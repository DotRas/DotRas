using System;
using System.Collections.Generic;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    public abstract class CallTraceEvent : TraceEvent
    {
        public IDictionary<string, object> Args { get; } = new Dictionary<string, object>();
        public IDictionary<string, object> OutArgs { get; } = new Dictionary<string, object>();
    }
}