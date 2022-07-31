using System;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class IPSecExceptionTests
    {
        [Test]
        public void InitializeWithDefaultConstructor()
        {
            Assert.DoesNotThrow(() => _ = new IPSecException());
        }

        [Test]
        public void InitializeWithCustomMessage()
        {
            var message = "This is a test message!";

            var target = new IPSecException(message);

            Assert.AreEqual(message, target.Message);
        }

        [Test]
        public void InitializeWithOnlyErrorCode()
        {
            var errorCode = 13000;

            var target = new IPSecException(errorCode);

            Assert.AreEqual(errorCode, target.NativeErrorCode);
        }

        [Test]
        public void InitializeWithErrorCodeAndCustomMessage()
        {
            var errorCode = 13000;
            var message = "This is a test message!";

            var target = new IPSecException(errorCode, message);

            Assert.AreEqual(errorCode, target.NativeErrorCode);
            Assert.AreEqual(message, target.Message);
        }

        [Test]
        public void InitializeWithCustomMessageAndInnerException()
        {
            var innerEx = new Exception("This is a test exception!");
            var message = "This is a test message!";

            var target = new IPSecException(message, innerEx);

            Assert.AreEqual(message, target.Message);
            Assert.AreEqual(innerEx, target.InnerException);
        }
    }
}