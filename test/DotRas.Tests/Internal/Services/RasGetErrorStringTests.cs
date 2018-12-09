using System;
using System.Text;
using DotRas.Internal.Services;
using DotRas.Win32;
using Moq;
using NUnit.Framework;
using static DotRas.Win32.WinError;

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class RasGetErrorStringTests
    {
        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasGetErrorString(null);
            });
        }

        [Test]
        public void ReturnNullWhenTheErrorCodeIsNotValid()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasGetErrorString(87, It.IsAny<StringBuilder>(), It.IsAny<int>())).Returns(ERROR_INVALID_PARAMETER);

            var target = new RasGetErrorString(api.Object);
            var result = target.GetErrorString(87);

            Assert.IsNull(result);
        }

        [Test]
        public void ReturnTheErrorString()
        {
            var message = "This is a test message!";

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasGetErrorString(1, It.IsAny<StringBuilder>(), It.IsAny<int>()))
                .Callback<int, StringBuilder, int>((o1, o2, o3) => { o2.Append(message); })
                .Returns(SUCCESS);

            var target = new RasGetErrorString(api.Object);
            var result = target.GetErrorString(1);

            Assert.AreEqual(message, result);
        }

        [Test]
        public void ThrowAnArgumentExceptionWhenTheErrorCodeIsZero()
        {
            var api = new Mock<IRasApi32>();

            var target = new RasGetErrorString(api.Object);
            Assert.Throws<ArgumentException>(() => target.GetErrorString(0));

            api.Verify(o => o.RasGetErrorString(It.IsAny<int>(), It.IsAny<StringBuilder>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void ThrowAnArgumentExceptionWhenTheErrorCodeIsLessThanZero()
        {
            var api = new Mock<IRasApi32>();

            var target = new RasGetErrorString(api.Object);
            Assert.Throws<ArgumentException>(() => target.GetErrorString(-1));

            api.Verify(o => o.RasGetErrorString(It.IsAny<int>(), It.IsAny<StringBuilder>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void ThrowAWin32ExceptionWhenTheBufferSizeIsTooSmall()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasGetErrorString(1, It.IsAny<StringBuilder>(), It.IsAny<int>())).Returns(ERROR_INSUFFICIENT_BUFFER);

            var target = new RasGetErrorString(api.Object);
            Assert.Throws<System.ComponentModel.Win32Exception>(() => target.GetErrorString(1));
        }        
    }
}