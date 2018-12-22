using System;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.ErrorHandling;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services.ErrorHandling
{
    [TestFixture]
    public class Win32ErrorInformationServiceTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheRasGetErrorStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Win32ErrorInformationService(null, new Mock<IWin32FormatMessage>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheWin32FormatMessageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Win32ErrorInformationService(new Mock<IRasGetErrorString>().Object, null));
        }

        [Test]
        public void ReturnsNullWhenTheErrorCodeIsZero()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();
            var win32FormatMessage = new Mock<IWin32FormatMessage>();

            var target = new Win32ErrorInformationService(rasGetErrorString.Object, win32FormatMessage.Object);
            var result = target.CreateFromErrorCode(0);

            Assert.IsNull(result);
        }

        [Test]
        public void ReturnsErrorMessageFromRasAsExpected()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();
            rasGetErrorString.Setup(o => o.GetErrorString(601)).Returns("ras");

            var win32FormatMessage = new Mock<IWin32FormatMessage>();

            var target = new Win32ErrorInformationService(rasGetErrorString.Object, win32FormatMessage.Object);
            var result = target.CreateFromErrorCode(601);

            Assert.AreEqual(601, result.ErrorCode);
            Assert.AreEqual("ras", result.Message);
        }

        [Test]
        public void ReturnsErrorMessageFromWin32AsExpected()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var win32FormatMessage = new Mock<IWin32FormatMessage>();
            win32FormatMessage.Setup(o => o.FormatMessage(6)).Returns("win32");

            var target = new Win32ErrorInformationService(rasGetErrorString.Object, win32FormatMessage.Object);
            var result = target.CreateFromErrorCode(6);

            Assert.AreEqual(6, result.ErrorCode);
            Assert.AreEqual("win32", result.Message);
        }

        [Test]
        public void ReturnsErrorMessageFromWin32WithFallThroughAsExpected()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var win32FormatMessage = new Mock<IWin32FormatMessage>();
            win32FormatMessage.Setup(o => o.FormatMessage(610)).Returns("win32");

            var target = new Win32ErrorInformationService(rasGetErrorString.Object, win32FormatMessage.Object);
            var result = target.CreateFromErrorCode(610);

            Assert.AreEqual(610, result.ErrorCode);
            Assert.AreEqual("win32", result.Message);

            rasGetErrorString.Verify(o => o.GetErrorString(610), Times.Once);        
        }
    }
}