using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Dialing;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services.Dialing;

[TestFixture]
public class RasDialExtensionsBuilderTests
{
    private Mock<IStructFactory> factory;
    private Mock<IRasGetEapUserData> getEapUserData;

    [SetUp]
    public void Init()
    {
        factory = new Mock<IStructFactory>();
        getEapUserData = new Mock<IRasGetEapUserData>();
    }

    [Test]
    public void ThrowsAnExceptionWhenStructFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasDialExtensionsBuilder(null, getEapUserData.Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenGetEapUserDataIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasDialExtensionsBuilder(factory.Object, null));
    }

    [Test]
    public void ThrowsAnExceptionWhenContextIsNull()
    {
        var target = new RasDialExtensionsBuilder(factory.Object, getEapUserData.Object);
        Assert.Throws<ArgumentNullException>(() => target.Build(null));
    }

    [Test]
    public void ConfiguresTheOwnerHandleAsExpected()
    {
        var expected = new IntPtr(1);

        var win32Window = new Mock<IWin32Window>();
        win32Window.Setup(o => o.Handle).Returns(expected);

        var target = new RasDialExtensionsBuilder(factory.Object, getEapUserData.Object);
        var result = target.Build(new RasDialContext
        {
            Options = new RasDialerOptions
            {
                Owner = win32Window.Object
            }
        });

        Assert.AreEqual(expected, result.hwndParent);
    }

    [Test]
    public void ConfiguresNoOptionsByDefault()
    {
        var target = new RasDialExtensionsBuilder(factory.Object, getEapUserData.Object);
        var result = target.Build(new RasDialContext());

        Assert.AreEqual(RDEOPT.None, result.dwfOptions);
    }
}