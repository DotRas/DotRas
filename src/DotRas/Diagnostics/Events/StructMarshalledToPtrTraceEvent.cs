using System;
using System.Collections.Generic;
using DotRas.Diagnostics.Tracing;
using DotRas.Diagnostics.Tracing.Formatters;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    [Formatter(typeof(StructMarshalledToPtrTraceEventFormatter))]
    public class StructMarshalledToPtrTraceEvent : CallCompletedTraceEvent
    {
        public IDictionary<string, object> Fields { get; } = new Dictionary<string, object>();
        public Type StructureType { get; set; }
        public IntPtr Result { get; set; }
    }
}