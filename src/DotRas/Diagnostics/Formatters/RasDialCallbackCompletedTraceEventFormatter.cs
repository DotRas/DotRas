using System;
using System.Text;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for a <see cref="RasDialCallbackCompletedTraceEvent"/> event.
    /// </summary>
    public class RasDialCallbackCompletedTraceEventFormatter : IFormatter<RasDialCallbackCompletedTraceEvent>
    {
        /// <inheritdoc />
        public string Format(RasDialCallbackCompletedTraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            var sb = new StringBuilder();

            sb.AppendLine("Callback received from RasDial. See the following for more details:");
            sb.AppendLine($"\tOccurred On: {eventData.OccurredOn}");
            sb.AppendLine($"\tResult: {eventData.Result}");

            sb.AppendLine("\tArguments:");
            foreach (var arg in eventData.Args)
            {
                sb.AppendLine($"\t\t{arg.Key}: [{arg.Value ?? "(null)"}]");
            }

            return sb.ToString();
        }
    }
}