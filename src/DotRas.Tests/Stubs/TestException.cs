using System;

namespace DotRas.Tests.Stubs
{
    [Serializable]
    public class TestException : Exception
    {
        public TestException() 
            : this("This is a test exception!")
        {
        }

        public TestException(string message) 
            : base(message)
        {
        }

        public TestException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

#if !NET7_0_OR_GREATER
        protected TestException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}