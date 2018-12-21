using System;
using System.Text;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Tracing.Formatters
{
    internal class PInvokeCallCompletedTraceEventFormatter<TEvent, TResult> : IFormatter<TEvent>
        where TEvent : PInvokeCallCompletedTraceEvent<TResult>
    {
        public string Format(TEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            var sb = new StringBuilder();

            sb.AppendLine("PInvoke external call completed. See the following for more details:");
            sb.AppendLine($"\tDllName: {eventData.DllName}");
            sb.AppendLine($"\tEntryPoint: '{eventData.MethodName}'");
            sb.AppendLine($"\tReturnCode: [{eventData.Result}]");
            sb.AppendLine($"\tDuration: {eventData.Duration}");

            sb.AppendLine("\tArguments:");
            foreach (var arg in eventData.Args)
            {
                sb.AppendLine($"\t\t{arg.Key}: [{arg.Value ?? "(null)"}]");
            }

            foreach (var arg in eventData.OutArgs)
            {
                sb.AppendLine($"\t\tout {arg.Key}: [{arg.Value ?? "(null)"}]");
            }

            return sb.ToString();
        }
    }
}