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
    }
}