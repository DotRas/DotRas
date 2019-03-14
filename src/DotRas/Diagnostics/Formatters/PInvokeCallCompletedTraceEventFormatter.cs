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
            sb.AppendLine().Append($"\tDllName: {eventData.DllName}");
            sb.AppendLine().Append($"\tEntryPoint: '{eventData.MethodName}'");
            sb.AppendLine().Append($"\tReturnCode: [{eventData.Result}]");
            sb.AppendLine().Append($"\tDuration: {eventData.Duration}");

            if (eventData.Args.Count > 0 || eventData.OutArgs.Count > 0)
            {
                sb.AppendLine().Append("\tArguments:");
                foreach (var arg in eventData.Args)
                {
                    sb.AppendLine().Append($"\t\t{arg.Key}: [{arg.Value ?? "(null)"}]");
                }

                foreach (var arg in eventData.OutArgs)
                {
                    sb.AppendLine().Append($"\t\tout {arg.Key}: [{arg.Value ?? "(null)"}]");
                }
            }

            return sb.ToString();
        }
    }
}