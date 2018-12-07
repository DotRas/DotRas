using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DotRas
{
    [Serializable]
    public class RasException : Win32Exception
    {
        public RasException(int error, string message)
            : base(error, message)
        {
        }

        protected RasException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}