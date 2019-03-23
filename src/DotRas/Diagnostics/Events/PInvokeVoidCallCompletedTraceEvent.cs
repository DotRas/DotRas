using System;
using DotRas.Diagnostics.Formatters;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    [EventFormatter(typeof(PInvokeVoidCallCompletedTraceEventFormatter))]
    public class PInvokeVoidCallCompletedTraceEvent : CallCompletedTraceEvent
    {
        /// <summary>
        /// Gets or sets the name of the DLL which contained the method.
        /// </summary>
        public string DllName { get; set; }

        /// <summary>
        /// Gets or sets the name of the method executed.
        /// </summary>
        public string MethodName { get; set; }
    }
}