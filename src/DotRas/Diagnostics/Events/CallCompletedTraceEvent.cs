using System;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    public abstract class CallCompletedTraceEvent : CallTraceEvent
    {
        public TimeSpan Duration { get; set; }    
    }
}