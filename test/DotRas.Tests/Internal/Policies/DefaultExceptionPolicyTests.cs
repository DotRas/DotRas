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
        [Test]
        public void ThrowsAnExceptionWhenGetErrorStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultExceptionPolicy(null));
        }

        [Test]
        public void ThrowsAnExceptionWhenErrorIsSuccess()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            Assert.Throws<ArgumentException>(() => target.Create(SUCCESS));
        }

        [Test]
        public void ReturnsNotSupportedExceptionWhenErrorIsInvalidSize()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_INVALID_SIZE);

            Assert.IsInstanceOf<NotSupportedException>(result);
        }

        [Test]
        public void ReturnsAWin32ExceptionWhenTheMessageIsNotFoundInRas()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(int.MinValue);

            rasGetErrorString.Verify(o => o.GetErrorString(int.MinValue), Times.Once);
            Assert.IsInstanceOf<Win32Exception>(result);
        }

        [Test]
        public void ReturnsARasExceptionWhenTheMessageIsFound()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();
            rasGetErrorString.Setup(o => o.GetErrorString(ERROR_BUFFER_TOO_SMALL)).Returns("The buffer is too small.").Verifiable();

            var target = new DefaultExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_BUFFER_TOO_SMALL);

            rasGetErrorString.Verify();
            Assert.IsInstanceOf<RasException>(result);
        }
    }
}