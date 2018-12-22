using System;
using DotRas.Diagnostics.Formatters;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes a callback completion event used by the RasDial Win32 API.
    /// </summary>
    [Serializable]
    [EventFormatter(typeof(RasDialCallbackCompletedTraceEventFormatter))]
    public class RasDialCallbackCompletedTraceEvent : CallbackTraceEvent
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public bool Result { get; set; }
    }
}