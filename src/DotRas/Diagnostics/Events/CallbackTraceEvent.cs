using System;
using System.Collections.Generic;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes a callback based trace event.
    /// </summary>
    [Serializable]
    public abstract class CallbackTraceEvent : TraceEvent
    {
        /// <summary>
        /// Gets or sets when the event occurred.
        /// </summary>
        public DateTime OccurredOn { get; set; }

        /// <summary>
        /// Gets a dictionary of arguments and their associated values.
        /// </summary>
        public IDictionary<string, object> Args { get; } = new Dictionary<string, object>();
    }
}