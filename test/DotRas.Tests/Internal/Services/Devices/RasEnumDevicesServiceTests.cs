using System;
using System.Linq;
using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Devices;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Devices
{
    [TestFixture]
    public class RasEnumDevicesServiceTests
    {
        private delegate int RasEnumDevicesCallback(
            RASDEVINFO[] lpRasConn,
            ref int lpCb,
            ref int lpcDevices);

        private Mock<IRasApi32> api;
        private Mock<IStructArrayFactory> structFactory;
        private Mock<IExceptionPolicy> exceptionPolicy;
        private Mock<IDeviceTypeFactory> deviceTypeFactory;

        [SetUp]
        public void Setup()
        {
            api = new Mock<IRasApi32>();
            structFactory = new Mock<IStructArrayFactory>();
            exceptionPolicy = new Mock<IExceptionPolicy>();
            deviceTypeFactory = new Mock<IDeviceTypeFactory>();
        }

        [Test]
        public void ThrowsAnExceptionWhenApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new RasEnumDevicesService(null, structFactory.Object, exceptionPolicy.Object, deviceTypeFactory.Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new RasEnumDevicesService(api.Object, null, exceptionPolicy.Object, deviceTypeFactory.Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new RasEnumDevicesService(api.Object, structFactory.Object, null, deviceTypeFactory.Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenDeviceTypeFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new RasEnumDevicesService(api.Object, structFactory.Object, exceptionPolicy.Object, null));
        }

        [Test]
        public void ReturnsNoConnectionAsExpected()
        {
            api.Setup(o => o.RasEnumDevices(It.IsAny<RASDEVINFO[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumDevicesCallback(
                (RASDEVINFO[] o1, ref int o2, ref int o3) =>
                {
                    o2 = 0;
                    o3 = 0;

                    return SUCCESS;
                }));

            structFactory.Setup(o => o.CreateArray<RASDEVINFO>(1, out It.Ref<int>.IsAny)).Returns(new RASDEVINFO[1]);

            var target = new RasEnumDevicesService(api.Object, structFactory.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
            var result = target.EnumerateDevices().ToArray();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ReturnsOneConnectionAsExpected()
        {
            var deviceName = "WAN";

            api.Setup(o => o.RasEnumDevices(It.IsAny<RASDEVINFO[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumDevicesCallback(
                (RASDEVINFO[] o1, ref int o2, ref int o3) =>
                {
                    o1[0].szDeviceName = deviceName;
                    o1[0].szDeviceType = RASDT_Vpn;

                    o2 = 1;
                    o3 = 1;

                    return SUCCESS;
                }));

            deviceTypeFactory.Setup(o => o.Create(deviceName, RASDT_Vpn)).Returns(new Vpn(deviceName));
            structFactory.Setup(o => o.CreateArray<RASDEVINFO>(1, out It.Ref<int>.IsAny)).Returns(new RASDEVINFO[1]);

            var target = new RasEnumDevicesService(api.Object, structFactory.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
            var result = target.EnumerateDevices().Single();

            Assert.IsNotNull(result);
        }

        [Test]
        public void ReturnsMultipleConnectionAsExpected()
        {
            var deviceName = "WAN";

            api.Setup(o => o.RasEnumDevices(It.IsAny<RASDEVINFO[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumDevicesCallback(
                (RASDEVINFO[] o1, ref int lpCb, ref int count) =>
                {
                    if (count == 1)
                    {
                        count = 2;

                        return ERROR_BUFFER_TOO_SMALL;
                    }

                    if (count == 2)
                    {
                        o1[0].szDeviceName = deviceName;
                        o1[0].szDeviceType = RASDT_Vpn;

                        o1[1].szDeviceName = deviceName;
                        o1[1].szDeviceType = RASDT_Vpn;

                        return SUCCESS;
                    }

                    return -1;
                }));

            deviceTypeFactory.Setup(o => o.Create(deviceName, RASDT_Vpn)).Returns(new Vpn(deviceName));

            structFactory.Setup(o => o.CreateArray<RASDEVINFO>(1, out It.Ref<int>.IsAny)).Returns(new RASDEVINFO[1]);
            structFactory.Setup(o => o.CreateArray<RASDEVINFO>(2, out It.Ref<int>.IsAny)).Returns(new RASDEVINFO[2]);

            var target = new RasEnumDevicesService(api.Object, structFactory.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
            var result = target.EnumerateDevices().ToArray();

            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheApiReturnsNonZero()
        {
            api.Setup(o => o.RasEnumDevices(It.IsAny<RASDEVINFO[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(ERROR_INVALID_PARAMETER);

            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());
            structFactory.Setup(o => o.CreateArray<RASCONN>(1, out It.Ref<int>.IsAny)).Returns(new RASCONN[1]);

            var target = new RasEnumDevicesService(api.Object, structFactory.Object, exceptionPolicy.Object, deviceTypeFactory.Object);
            Assert.Throws<TestException>(() => target.EnumerateDevices().ToArray());
        }
    }
}