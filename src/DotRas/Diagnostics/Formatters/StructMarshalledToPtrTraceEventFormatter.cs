using System;
using System.Text;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for a <see cref="StructMarshalledToPtrTraceEvent"/> event.
    /// </summary>
    public class StructMarshalledToPtrTraceEventFormatter : IEventFormatter<StructMarshalledToPtrTraceEvent>
    {
        /// <inheritdoc />
        public string Format(StructMarshalledToPtrTraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            var sb = new StringBuilder();

            sb.Append("Structure marshalled to pointer. See the following for more details:");
            sb.Append($"\n\tType: {eventData.StructureType.FullName}");
            sb.Append($"\n\tResult: {eventData.Result}");
            sb.Append($"\n\tDuration: {eventData.Duration}");

            if (eventData.Fields.Count > 0)
            {
                sb.Append("\n\tFields:");
                foreach (var field in eventData.Fields)
                {
                    sb.Append($"\n\t\t{field.Key}: [{field.Value ?? "(null)"}]");
                }
            }

            return sb.ToString();
        }
    }
}