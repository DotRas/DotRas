using System;

namespace DotRas.Diagnostics.Events
{
    [Serializable]
    public class PInvokeCallCompletedTraceEvent<TResult> : CallCompletedTraceEvent
    {
        public TResult Result { get; set; }
        public string DllName { get; set; }
        public string MethodName { get; set; }
    }
}