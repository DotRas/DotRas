using System;
using System.Runtime.Serialization;

namespace DotRas.Internal.DependencyInjection
{
    [Serializable]
    internal class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string message) 
            : base(message)
        {
        }

        protected ServiceNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}