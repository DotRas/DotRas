using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services.Connections
{
    [TestFixture]
    public class RasGetConnectionStatusServiceTests
    {
        [Test]
        public void ThrowsAnExceptionWhenApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusService(
                null, 
                new Mock<IStructFactory>().Object, 
                new Mock<IWin32ErrorInformation>().Object, 
                new Mock<IIPAddressConverter>().Object,
                new Mock<IExceptionPolicy>().Object, 
                new Mock<IDeviceTypeFactory>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusService(
                new Mock<IRasApi32>().Object,
                null,
                new Mock<IWin32ErrorInformation>().Object,
                new Mock<IIPAddressConverter>().Object,
                new Mock<IExceptionPolicy>().Object,
                new Mock<IDeviceTypeFactory>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenErrorInformationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusService(
                new Mock<IRasApi32>().Object,
                new Mock<IStructFactory>().Object,
                null,
                new Mock<IIPAddressConverter>().Object,
                new Mock<IExceptionPolicy>().Object,
                new Mock<IDeviceTypeFactory>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenIPAddressConverterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusService(
                new Mock<IRasApi32>().Object,
                new Mock<IStructFactory>().Object,
                new Mock<IWin32ErrorInformation>().Object,
                null,
                new Mock<IExceptionPolicy>().Object,
                new Mock<IDeviceTypeFactory>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusService(
                new Mock<IRasApi32>().Object,
                new Mock<IStructFactory>().Object,
                new Mock<IWin32ErrorInformation>().Object,
                new Mock<IIPAddressConverter>().Object,
                null,
                new Mock<IDeviceTypeFactory>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenDeviceTypeFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusService(
                new Mock<IRasApi32>().Object,
                new Mock<IStructFactory>().Object,
                new Mock<IWin32ErrorInformation>().Object,
                new Mock<IIPAddressConverter>().Object,
                new Mock<IExceptionPolicy>().Object,
                null));
        }

        [Test]
        public void ThrowsAnExceptionWhenHandleIsNull()
        {
            var api = new Mock<IRasApi32>();
            var structFactory = new Mock<IStructFactory>();
            var win32ErrorInformation = new Mock<IWin32ErrorInformation>();
            var ipAddressConverter = new Mock<IIPAddressConverter>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();

            var target = new RasGetConnectStatusService(api.Object, structFactory.Object, win32ErrorInformation.Object, ipAddressConverter.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
            Assert.Throws<ArgumentNullException>(() => target.GetConnectionStatus(null));
        }
    }
}