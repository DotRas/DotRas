using System;
using System.Text;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for a <see cref="PInvokeCallCompletedTraceEvent{TResult}"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of event being formatted.</typeparam>
    /// <typeparam name="TResult">The result of the event.</typeparam>
    public class PInvokeCallCompletedTraceEventFormatter<TEvent, TResult> : IEventFormatter<TEvent>
        where TEvent : PInvokeCallCompletedTraceEvent<TResult>
    {
        /// <inheritdoc />
        public string Format(TEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            var sb = new StringBuilder();

            sb.Append("PInvoke external call completed. See the following for more details:");
            sb.Append($"\n\tDllName: {eventData.DllName}");
            sb.Append($"\n\tEntryPoint: '{eventData.MethodName}'");
            sb.Append($"\n\tReturnCode: [{eventData.Result}]");
            sb.Append($"\n\tDuration: {eventData.Duration}");

            if (eventData.Args.Count > 0 || eventData.OutArgs.Count > 0)
            {
                sb.Append("\n\tArguments:");
                foreach (var arg in eventData.Args)
                {
                    sb.Append($"\n\t\t{arg.Key}: [{arg.Value ?? "(null)"}]");
                }

                foreach (var arg in eventData.OutArgs)
                {
                    sb.Append($"\n\t\tout {arg.Key}: [{arg.Value ?? "(null)"}]");
                }
            }

            return sb.ToString();
        }
    }
}