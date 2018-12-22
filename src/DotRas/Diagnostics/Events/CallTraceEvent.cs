using System;
using System.Collections.Generic;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes a call trace event.
    /// </summary>
    [Serializable]
    public abstract class CallTraceEvent : TraceEvent
    {
        /// <summary>
        /// Gets a dictionary of arguments and their associated values.
        /// </summary>
        public IDictionary<string, object> Args { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets a dictionary of output arguments and their associated values.
        /// </summary>
        public IDictionary<string, object> OutArgs { get; } = new Dictionary<string, object>();
    }
}