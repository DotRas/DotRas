using System;
using System.Text;
using DotRas.Diagnostics.Events;

namespace DotRas.Diagnostics.Formatters
{
    /// <summary>
    /// Provides a formatter for a <see cref="StructMarshalledToPtrTraceEvent"/> event.
    /// </summary>
    public class StructMarshalledToPtrTraceEventFormatter : IFormatter<StructMarshalledToPtrTraceEvent>
    {
        /// <inheritdoc />
        public string Format(StructMarshalledToPtrTraceEvent eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            var sb = new StringBuilder();

            sb.AppendLine("Structure marshalled to pointer. See the following for more details:");
            sb.AppendLine($"\tType: {eventData.StructureType.FullName}");
            sb.AppendLine($"\tResult: {eventData.Result}");
            sb.AppendLine($"\tDuration: {eventData.Duration}");

            sb.AppendLine("\tFields:");
            foreach (var field in eventData.Fields)
            {
                sb.AppendLine($"\t\t{field.Key}: [{field.Value ?? "(null)"}]");
            }

            return sb.ToString();
        }
    }
}