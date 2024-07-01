using System;
using System.Text;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for a <see cref="RasDialCallbackCompletedTraceEvent"/> event.
    /// </summary>
    public class RasDialCallbackCompletedTraceEventFormatter : IEventFormatter<RasDialCallbackCompletedTraceEvent>
    {
        /// <inheritdoc />
        public string Format(RasDialCallbackCompletedTraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            var sb = new StringBuilder();

            sb.Append("Callback received from RasDial. See the following for more details:");
            sb.Append($"\n\tOccurred On: {eventData.OccurredOn}");
            sb.Append($"\n\tResult: {eventData.Result}");

            if (eventData.Args.Count > 0)
            {
                sb.Append("\n\tArguments:");
                foreach (var arg in eventData.Args)
                {
                    sb.Append($"\n\t\t{arg.Key}: [{arg.Value ?? "(null)"}]");
                }
            }

            return sb.ToString();
        }
    }
}