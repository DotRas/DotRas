using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Security;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Tests.Internal.Services.Security;

[TestFixture]
public class RasGetCredentialsServiceTests
{
    private delegate void RasGetCredentialsCallback(
        string lpszPhoneBook,
        string lpszEntryName,
        ref RASCREDENTIALS lpCredentials);

    [Test]
    public void ThrowsAnExceptionWhenTheApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetCredentialsService(null, new Mock<IStructFactory>().Object, new Mock<IExceptionPolicy>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheStructFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetCredentialsService(new Mock<IRasApi32>().Object, null, new Mock<IExceptionPolicy>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheExceptionPolicyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetCredentialsService(new Mock<IRasApi32>().Object, new Mock<IStructFactory>().Object, null));
    }

    [Test]
    public void ReturnsTheNetworkCredentialAsExpected()
    {
        var api = new Mock<IRasApi32>();
        api.Setup(o => o.RasGetCredentials("PATH", "ENTRY", ref It.Ref<RASCREDENTIALS>.IsAny)).Callback(new RasGetCredentialsCallback(
            (string o1, string o2, ref RASCREDENTIALS o3) =>
            {
                o3.szUserName = "USER";
                o3.szPassword = "PASSWORD";
                o3.szDomain = "DOMAIN";
            }));

        var structFactory = new Mock<IStructFactory>();
        structFactory.Setup(o => o.Create<RASCREDENTIALS>()).Returns(new RASCREDENTIALS());

        var exceptionPolicy = new Mock<IExceptionPolicy>();

        var target = new RasGetCredentialsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.GetNetworkCredential("ENTRY", "PATH");

        Assert.AreEqual("USER", result.UserName);
        Assert.AreEqual("PASSWORD", result.Password);
        Assert.AreEqual("DOMAIN", result.Domain);
    }

    [Test]
    public void ThrowsAnExceptionForANonZeroResultCode()
    {
        var api = new Mock<IRasApi32>();
        api.Setup(o => o.RasGetCredentials("PATH", "ENTRY", ref It.Ref<RASCREDENTIALS>.IsAny)).Returns(-1);

        var structFactory = new Mock<IStructFactory>();
        structFactory.Setup(o => o.Create<RASCREDENTIALS>()).Returns(new RASCREDENTIALS());

        var exceptionPolicy = new Mock<IExceptionPolicy>();
        exceptionPolicy.Setup(o => o.Create(-1)).Returns(new TestException());

        var target = new RasGetCredentialsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.Throws<TestException>(() => target.GetNetworkCredential("ENTRY", "PATH"));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheEntryNameIsNull()
    {
        var api = new Mock<IRasApi32>();
        var structFactory = new Mock<IStructFactory>();
        var exceptionPolicy = new Mock<IExceptionPolicy>();

        var target = new RasGetCredentialsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.Throws<ArgumentNullException>(() => target.GetNetworkCredential(null, "PATH"));
    }

    [Test]
    public void DoesNotThrowAnExceptionWhenThePhoneBookPathIsNull()
    {
        var api = new Mock<IRasApi32>();
        var structFactory = new Mock<IStructFactory>();
        var exceptionPolicy = new Mock<IExceptionPolicy>();

        var target = new RasGetCredentialsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.DoesNotThrow(() => target.GetNetworkCredential("ENTRY", null));
    }
}