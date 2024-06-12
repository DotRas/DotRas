using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Policies {
    [TestFixture]
    public class DefaultExceptionPolicyTests {
        private Mock<IRasGetErrorString> rasGetErrorString;

        [SetUp]
        public void Init() => rasGetErrorString = new Mock<IRasGetErrorString>();

        [Test]
        public void ThrowsAnExceptionWhenGetErrorStringIsNull() => Assert.Throws<ArgumentNullException>(() => _ = new DefaultExceptionPolicy(null));

        [Test]
        public void ThrowsAnExceptionWhenErrorIsSuccess() {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            Assert.Throws<ArgumentException>(() => target.Create(SUCCESS));
        }

        [Test]
        public void ReturnsNotSupportedExceptionWhenErrorIsInvalidSize() {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_INVALID_SIZE);

            Assert.That(result, Is.InstanceOf<OperatingSystemNotSupportedException>());
        }

        [Test]
        public void ReturnsAnUnknownErrorForRasExceptionsWithNoMessage() {
            rasGetErrorString.Setup(o => o.GetErrorString(600)).Returns("").Verifiable();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(600) as RasException;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Unknown error."));
        }

        [Test]
        public void ReturnsAWin32ExceptionWhenTheMessageIsNotFoundInRas() {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(int.MinValue);

            rasGetErrorString.Verify(o => o.GetErrorString(int.MinValue), Times.Never);
            Assert.That(result, Is.InstanceOf<Win32Exception>());
        }

        [Test]
        public void ReturnsARasExceptionWhenTheMessageIsFound() {
            rasGetErrorString.Setup(o => o.GetErrorString(ERROR_BUFFER_TOO_SMALL)).Returns("The buffer is too small.").Verifiable();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_BUFFER_TOO_SMALL);

            rasGetErrorString.Verify();
            Assert.That(result, Is.InstanceOf<RasException>());
        }

        [Test]
        public void ReturnsAnIPSecExceptionForIkeCredentialsUnacceptableErrorCode() {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(13801);

            Assert.That(result, Is.InstanceOf<IPSecException>());
            Assert.That(result.Message, Does.Contain("IKE"));
        }
    }
}
