using System;
using System.Runtime.Serialization;

namespace DotRas.Diagnostics.Tracing
{
    [Serializable]
    internal class FormatterNotFoundException : Exception
    {
        public FormatterNotFoundException(string message) 
            : base(message)
        {
        }

        protected FormatterNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}