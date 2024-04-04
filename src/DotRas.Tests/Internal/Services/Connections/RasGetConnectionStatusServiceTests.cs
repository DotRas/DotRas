using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Connections;

[TestFixture]
public class RasGetConnectionStatusServiceTests
{
    private delegate int RasGetConnectStatusCallback(IntPtr handle, ref RASCONNSTATUS rasConnStatus);

    private Mock<IRasApi32> api;
    private Mock<IStructFactory> structFactory;
    private Mock<IIPAddressConverter> ipAddressConverter;
    private Mock<IExceptionPolicy> exceptionPolicy;
    private Mock<IDeviceTypeFactory> deviceTypeFactory;

    private IntPtr handle;
    private Mock<IRasConnection> connection;

    private Mock<RasDevice> device;

    [SetUp]
    public void Init()
    {
        api = new Mock<IRasApi32>();
        structFactory = new Mock<IStructFactory>();
        ipAddressConverter = new Mock<IIPAddressConverter>();
        exceptionPolicy = new Mock<IExceptionPolicy>();

        device = new Mock<RasDevice>();
        deviceTypeFactory = new Mock<IDeviceTypeFactory>();
        deviceTypeFactory.Setup(o => o.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(device.Object);

        structFactory.Setup(o => o.Create<RASCONNSTATUS>()).Returns(new RASCONNSTATUS());

        handle = new IntPtr(1);

        connection = new Mock<IRasConnection>();
        connection.Setup(o => o.Handle).Returns(handle);
    }

    [Test]
    public void ThrowsAnExceptionWhenApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasGetConnectStatusService(
            null,
            new Mock<IStructFactory>().Object,
            new Mock<IIPAddressConverter>().Object,
            new Mock<IExceptionPolicy>().Object,
            new Mock<IDeviceTypeFactory>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenStructFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasGetConnectStatusService(
            new Mock<IRasApi32>().Object,
            null,
            new Mock<IIPAddressConverter>().Object,
            new Mock<IExceptionPolicy>().Object,
            new Mock<IDeviceTypeFactory>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenIpAddressConverterIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasGetConnectStatusService(
            new Mock<IRasApi32>().Object,
            new Mock<IStructFactory>().Object,
            null,
            new Mock<IExceptionPolicy>().Object,
            new Mock<IDeviceTypeFactory>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasGetConnectStatusService(
            new Mock<IRasApi32>().Object,
            new Mock<IStructFactory>().Object,
            new Mock<IIPAddressConverter>().Object,
            null,
            new Mock<IDeviceTypeFactory>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenDeviceTypeFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new RasGetConnectStatusService(
            new Mock<IRasApi32>().Object,
            new Mock<IStructFactory>().Object,
            new Mock<IIPAddressConverter>().Object,
            new Mock<IExceptionPolicy>().Object,
            null));
    }

    [Test]
    public void ThrowsAnExceptionWhenHandleIsNull()
    {
        var target = new RasGetConnectStatusService(api.Object, structFactory.Object, ipAddressConverter.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
        Assert.Throws<ArgumentNullException>(() => target.GetConnectionStatus(null));
    }

    [Test]
    public void ThrowsAnExceptionWhenApiReturnsNonZero()
    {
        exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_HANDLE)).Returns(new TestException());

        api.Setup(o => o.RasGetConnectStatus(handle, ref It.Ref<RASCONNSTATUS>.IsAny)).Returns(
            new RasGetConnectStatusCallback((IntPtr h, ref RASCONNSTATUS rasConnStatus) => ERROR_INVALID_HANDLE));

        var target = new RasGetConnectStatusService(api.Object, structFactory.Object, ipAddressConverter.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
        Assert.Throws<TestException>(() => target.GetConnectionStatus(connection.Object));
    }

    [Test]
    public void ErrorCodeIsNullWhenValueIsZero()
    {
        api.Setup(o => o.RasGetConnectStatus(handle, ref It.Ref<RASCONNSTATUS>.IsAny)).Returns(new RasGetConnectStatusCallback(
                (IntPtr h, ref RASCONNSTATUS rasConnStatus) =>
                {
                    rasConnStatus.dwError = 0;
                    return SUCCESS;
                }));

        var target = new RasGetConnectStatusService(api.Object, structFactory.Object, ipAddressConverter.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
        var result = target.GetConnectionStatus(connection.Object);

        Assert.Null(result.ErrorCode);
    }

    [Test]
    public void ErrorCodeIsSetWhenValueIsNonZero()
    {
        api.Setup(o => o.RasGetConnectStatus(handle, ref It.Ref<RASCONNSTATUS>.IsAny)).Returns(new RasGetConnectStatusCallback(
            (IntPtr h, ref RASCONNSTATUS rasConnStatus) =>
            {
                rasConnStatus.dwError = 1;
                return SUCCESS;
            }));

        var target = new RasGetConnectStatusService(api.Object, structFactory.Object, ipAddressConverter.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
        var result = target.GetConnectionStatus(connection.Object);

        Assert.AreEqual(1, result.ErrorCode);
    }
}