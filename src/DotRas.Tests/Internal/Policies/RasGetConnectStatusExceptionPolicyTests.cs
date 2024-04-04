using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Policies;

[TestFixture]
public class RasGetConnectStatusExceptionPolicyTests
{
    [Test]
    public void ThrowsAnExceptionWhenGetErrorStringIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusExceptionPolicy(null));
    }

    [Test]
    public void ShouldTranslateTheErrorCodeAsExpected()
    {
        var rasGetErrorString = new Mock<IRasGetErrorString>();
        rasGetErrorString.Setup(o => o.GetErrorString(ERROR_NO_CONNECTION)).Returns("No connection");

        var target = new RasGetConnectStatusExceptionPolicy(rasGetErrorString.Object);
        var result = target.Create(ERROR_INVALID_HANDLE);

        Assert.IsInstanceOf<RasException>(result);

        var ex = (RasException)result;
        Assert.AreEqual(ERROR_NO_CONNECTION, ex.NativeErrorCode);
        Assert.AreEqual("No connection", ex.Message);

        Assert.True(target.TranslatedToNoConnection);
    }
}