using System;
using System.Collections.Generic;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    public abstract class CallbackTraceEvent : TraceEvent
    {
        public DateTime OccurredOn { get; set; }
        public IDictionary<string, object> Args { get; } = new Dictionary<string, object>();
    }
}