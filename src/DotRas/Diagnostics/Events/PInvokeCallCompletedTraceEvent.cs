using System;

namespace DotRas.Diagnostics.Events
{
    /// <summary>
    /// Describes an event in which a platform invocation (aka P/Invoke) call has completed.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the platform invocation call.</typeparam>
    [Serializable]
    public class PInvokeCallCompletedTraceEvent<TResult> : CallCompletedTraceEvent
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public TResult Result { get; set; }

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