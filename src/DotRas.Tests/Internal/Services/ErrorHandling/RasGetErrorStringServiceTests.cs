using System.Text;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.ErrorHandling;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.ErrorHandling;

[TestFixture]
public class RasGetErrorStringServiceTests
{
    [Test]
    public void ThrowAnExceptionWhenTheApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new RasGetErrorStringService(null);
        });
    }

    [Test]
    public void ReturnNullWhenTheErrorCodeIsNotValid()
    {
        var api = new Mock<IRasApi32>();
        api.Setup(o => o.RasGetErrorString(87, It.IsAny<StringBuilder>(), It.IsAny<int>())).Returns(ERROR_INVALID_PARAMETER);

        var target = new RasGetErrorStringService(api.Object);
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

        var target = new RasGetErrorStringService(api.Object);
        var result = target.GetErrorString(1);

        Assert.AreEqual(message, result);
    }

    [Test]
    public void MustNotThrowAnExceptionWhenTheErrorCodeIsZero()
    {
        var api = new Mock<IRasApi32>();

        var target = new RasGetErrorStringService(api.Object);
        Assert.DoesNotThrow(() => target.GetErrorString(0));

        api.Verify(o => o.RasGetErrorString(It.IsAny<int>(), It.IsAny<StringBuilder>(), It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void MustNotThrowAnExceptionWhenTheErrorCodeIsLessThanZero()
    {
        var api = new Mock<IRasApi32>();

        var target = new RasGetErrorStringService(api.Object);
        Assert.DoesNotThrow(() => target.GetErrorString(-1));

        api.Verify(o => o.RasGetErrorString(It.IsAny<int>(), It.IsAny<StringBuilder>(), It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void ThrowAWin32ExceptionWhenTheBufferSizeIsTooSmall()
    {
        var api = new Mock<IRasApi32>();
        api.Setup(o => o.RasGetErrorString(1, It.IsAny<StringBuilder>(), It.IsAny<int>())).Returns(ERROR_INSUFFICIENT_BUFFER);

        var target = new RasGetErrorStringService(api.Object);
        Assert.Throws<System.ComponentModel.Win32Exception>(() => target.GetErrorString(1));
    }
}