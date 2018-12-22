using System;
using System.Collections.Generic;
using DotRas.Diagnostics.Formatters;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes a structure which has been marshalled to a pointer.
    /// </summary>
    [Serializable]
    [EventFormatter(typeof(StructMarshalledToPtrTraceEventFormatter))]
    public class StructMarshalledToPtrTraceEvent : CallCompletedTraceEvent
    {
        /// <summary>
        /// Gets or sets a dictionary of fields and their associated values.
        /// </summary>
        public IDictionary<string, object> Fields { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the type of structure.
        /// </summary>
        public Type StructureType { get; set; }

        /// <summary>
        /// Gets or sets the resulting pointer.
        /// </summary>
        public IntPtr Result { get; set; }
    }
}