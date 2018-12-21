using System;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes an event in which a call has completed.
    /// </summary>
    [Serializable]
    public abstract class CallCompletedTraceEvent : CallTraceEvent
    {
        /// <summary>
        /// Gets or sets the duration of time which the call took to execute.
        /// </summary>
        public TimeSpan Duration { get; set; }    
    }
}