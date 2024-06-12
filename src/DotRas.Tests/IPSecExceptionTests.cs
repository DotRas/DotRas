using NUnit.Framework;
using System;

namespace DotRas.Tests {
    [TestFixture]
    public class IPSecExceptionTests {
        [Test]
        public void InitializeWithDefaultConstructor() => Assert.DoesNotThrow(() => _ = new IPSecException());

        [Test]
        public void InitializeWithCustomMessage() {
            var message = "This is a test message!";

            var target = new IPSecException(message);

            Assert.That(target.Message, Is.EqualTo(message));
        }

        [Test]
        public void InitializeWithOnlyErrorCode() {
            var errorCode = 13000;

            var target = new IPSecException(errorCode);

            Assert.That(target.NativeErrorCode, Is.EqualTo(errorCode));
        }

        [Test]
        public void InitializeWithErrorCodeAndCustomMessage() {
            var errorCode = 13000;
            var message = "This is a test message!";

            var target = new IPSecException(errorCode, message);

            Assert.Multiple(() => {
                Assert.That(target.NativeErrorCode, Is.EqualTo(errorCode));
                Assert.That(target.Message, Is.EqualTo(message));
            });
        }

        [Test]
        public void InitializeWithCustomMessageAndInnerException() {
            var innerEx = new Exception("This is a test exception!");
            var message = "This is a test message!";

            var target = new IPSecException(message, innerEx);

            Assert.Multiple(() => {
                Assert.That(target.Message, Is.EqualTo(message));
                Assert.That(target.InnerException, Is.EqualTo(innerEx));
            });
        }
    }
}
