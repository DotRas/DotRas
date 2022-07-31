using System;
using System.ComponentModel;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Policies
{
    [TestFixture]
    public class DefaultExceptionPolicyTests
    {
        private Mock<IRasGetErrorString> rasGetErrorString;

        [SetUp]
        public void Init()
        {
            rasGetErrorString = new Mock<IRasGetErrorString>();
        }

        [Test]
        public void ThrowsAnExceptionWhenGetErrorStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new DefaultExceptionPolicy(null));
        }

        [Test]
        public void ThrowsAnExceptionWhenErrorIsSuccess()
        {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            Assert.Throws<ArgumentException>(() => target.Create(SUCCESS));
        }

        [Test]
        public void ReturnsNotSupportedExceptionWhenErrorIsInvalidSize()
        {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_INVALID_SIZE);

            Assert.IsInstanceOf<OperatingSystemNotSupportedException>(result);
        }

        [Test]
        public void ReturnsAnUnknownErrorForRasExceptionsWithNoMessage()
        {
            rasGetErrorString.Setup(o => o.GetErrorString(600)).Returns("").Verifiable();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(600) as RasException;

            Assert.IsNotNull(result);
            Assert.AreEqual("Unknown error.", result.Message);
        }

        [Test]
        public void ReturnsAWin32ExceptionWhenTheMessageIsNotFoundInRas()
        {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(int.MinValue);

            rasGetErrorString.Verify(o => o.GetErrorString(int.MinValue), Times.Never);
            Assert.IsInstanceOf<Win32Exception>(result);
        }

        [Test]
        public void ReturnsARasExceptionWhenTheMessageIsFound()
        {
            rasGetErrorString.Setup(o => o.GetErrorString(ERROR_BUFFER_TOO_SMALL)).Returns("The buffer is too small.")
                .Verifiable();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_BUFFER_TOO_SMALL);

            rasGetErrorString.Verify();
            Assert.IsInstanceOf<RasException>(result);
        }

        [Test]
        public void ReturnsAnIPSecExceptionForIkeCredentialsUnacceptableErrorCode()
        {
            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(13801);

            Assert.IsInstanceOf<IPSecException>(result);
            StringAssert.Contains("IKE", result.Message);
        }
    }
}