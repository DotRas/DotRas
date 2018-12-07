using System;

namespace DotRas.Diagnostics.Tracing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    internal sealed class FormatterAttribute : Attribute
    {
        public Type FormatterType { get; }

        public FormatterAttribute(Type formatterType)
        {
            FormatterType = formatterType ?? throw new ArgumentNullException(nameof(formatterType));
        }
    }
}