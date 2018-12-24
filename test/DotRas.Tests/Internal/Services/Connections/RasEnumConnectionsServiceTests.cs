using System;
using System.Collections.ObjectModel;
using System.Linq;
using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection.Factories;
using DotRas.Internal.Interop;
using DotRas.Internal.Services;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Internal.Stubs;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Connections
{
    [TestFixture]
    public class RasEnumConnectionsServiceTests
    {
        private delegate int RasEnumConnectionsCallback(
            RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);

        private delegate RASCONN[] StructFactoryCallback(
            int count, 
            out int size);

        [Test]
        public void ThrowsAnExceptionWhenApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasEnumConnectionsService(null, new Mock<IDeviceTypeFactory>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IStructArrayFactory>().Object, new Mock<IServiceProvider>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenDeviceTypeFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasEnumConnectionsService(new Mock<IRasApi32>().Object, null, new Mock<IExceptionPolicy>().Object, new Mock<IStructArrayFactory>().Object, new Mock<IServiceProvider>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasEnumConnectionsService(new Mock<IRasApi32>().Object, new Mock<IDeviceTypeFactory>().Object, null, new Mock<IStructArrayFactory>().Object, new Mock<IServiceProvider>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasEnumConnectionsService(new Mock<IRasApi32>().Object, new Mock<IDeviceTypeFactory>().Object, new Mock<IExceptionPolicy>().Object, null, new Mock<IServiceProvider>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenServiceLocatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasEnumConnectionsService(new Mock<IRasApi32>().Object, new Mock<IDeviceTypeFactory>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IStructArrayFactory>().Object, null));
        }

        [Test]
        public void ReturnsNoConnectionAsExpected()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasEnumConnections(It.IsAny<RASCONN[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumConnectionsCallback(
                (RASCONN[] o1, ref int o2, ref int o3) =>
                {
                    o2 = 0;
                    o3 = 0;

                    return SUCCESS;
                }));

            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var structFactory = new Mock<IStructArrayFactory>();
            structFactory.Setup(o => o.CreateArray<RASCONN>(1, out It.Ref<int>.IsAny)).Returns(new RASCONN[1]);

            var serviceLocator = new Mock<IServiceProvider>();

            var target = new RasEnumConnectionsService(api.Object, deviceTypeFactory.Object, exceptionPolicy.Object, structFactory.Object, serviceLocator.Object);
            var result = target.EnumerateConnections().ToArray();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ReturnsOneConnectionAsExpected()
        {
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";
            var deviceName = "WAN";

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasEnumConnections(It.IsAny<RASCONN[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumConnectionsCallback(
                (RASCONN[] o1, ref int o2, ref int o3) =>
                {
                    o1[0].hrasconn = new IntPtr(1);
                    o1[0].szDeviceName = deviceName;
                    o1[0].szDeviceType = RASDT_Vpn;
                    o1[0].szEntryName = entryName;
                    o1[0].szPhonebook = phoneBookPath;

                    o2 = 1;
                    o3 = 1;

                    return SUCCESS;
                }));

            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();
            deviceTypeFactory.Setup(o => o.Create(deviceName, RASDT_Vpn)).Returns(new Vpn(deviceName));

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var structFactory = new Mock<IStructArrayFactory>();
            structFactory.Setup(o => o.CreateArray<RASCONN>(1, out It.Ref<int>.IsAny)).Returns(new RASCONN[1]);

            var getConnectStatus = new Mock<IRasGetConnectStatus>();
            var getConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var hangUp = new Mock<IRasHangUp>();

            var serviceLocator = new Mock<IServiceProvider>();
            serviceLocator.Setup(o => o.GetService(typeof(IRasGetConnectStatus))).Returns(getConnectStatus.Object);
            serviceLocator.Setup(o => o.GetService(typeof(IRasGetConnectionStatistics))).Returns(getConnectionStatistics.Object);
            serviceLocator.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(hangUp.Object);

            var target = new RasEnumConnectionsService(api.Object, deviceTypeFactory.Object, exceptionPolicy.Object, structFactory.Object, serviceLocator.Object);
            var result = target.EnumerateConnections().Single();

            Assert.IsNotNull(result);
        }

        [Test]
        public void ReturnsMultipleConnectionAsExpected()
        {
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";
            var deviceName = "WAN";

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasEnumConnections(It.IsAny<RASCONN[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumConnectionsCallback(
                (RASCONN[] o1, ref int lpCb, ref int count) =>
                {
                    if (count == 1)
                    {
                        count = 2;

                        return ERROR_BUFFER_TOO_SMALL;
                    }

                    if (count == 2)
                    {
                        o1[0].hrasconn = new IntPtr(1);
                        o1[0].szDeviceName = deviceName;
                        o1[0].szDeviceType = RASDT_Vpn;
                        o1[0].szEntryName = entryName;
                        o1[0].szPhonebook = phoneBookPath;

                        o1[1].hrasconn = new IntPtr(2);
                        o1[1].szDeviceName = deviceName;
                        o1[1].szDeviceType = RASDT_Vpn;
                        o1[1].szEntryName = entryName;
                        o1[1].szPhonebook = phoneBookPath;

                        return SUCCESS;
                    }

                    return -1;
                }));

            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();
            deviceTypeFactory.Setup(o => o.Create(deviceName, RASDT_Vpn)).Returns(new Vpn(deviceName));

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var structFactory = new StructFactory(new MarshallingService());

            var getConnectStatus = new Mock<IRasGetConnectStatus>();
            var getConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var hangUp = new Mock<IRasHangUp>();

            var serviceLocator = new Mock<IServiceProvider>();
            serviceLocator.Setup(o => o.GetService(typeof(IRasGetConnectStatus))).Returns(getConnectStatus.Object);
            serviceLocator.Setup(o => o.GetService(typeof(IRasGetConnectionStatistics))).Returns(getConnectionStatistics.Object);
            serviceLocator.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(hangUp.Object);

            var target = new RasEnumConnectionsService(api.Object, deviceTypeFactory.Object, exceptionPolicy.Object, structFactory, serviceLocator.Object);
            var result = target.EnumerateConnections().ToArray();

            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheApiReturnsNonZero()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasEnumConnections(It.IsAny<RASCONN[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(ERROR_INVALID_PARAMETER);

            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var structFactory = new Mock<IStructArrayFactory>();
            structFactory.Setup(o => o.CreateArray<RASCONN>(1, out It.Ref<int>.IsAny)).Returns(new RASCONN[1]);

            var serviceLocator = new Mock<IServiceProvider>();

            var target = new RasEnumConnectionsService(api.Object, deviceTypeFactory.Object, exceptionPolicy.Object, structFactory.Object, serviceLocator.Object);
            Assert.Throws<TestException>(() => target.EnumerateConnections().ToArray());
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleReturnsIsNull()
        {
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";
            var deviceName = "WAN";

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasEnumConnections(It.IsAny<RASCONN[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumConnectionsCallback(
                (RASCONN[] o1, ref int o2, ref int o3) =>
                {
                    o1[0].hrasconn = new IntPtr(1);
                    o1[0].szDeviceName = deviceName;
                    o1[0].szDeviceType = RASDT_Vpn;
                    o1[0].szEntryName = entryName;
                    o1[0].szPhonebook = phoneBookPath;

                    o2 = 1;
                    o3 = 1;

                    return SUCCESS;
                }));

            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var structFactory = new Mock<IStructArrayFactory>();
            structFactory.Setup(o => o.CreateArray<RASCONN>(1, out It.Ref<int>.IsAny)).Returns(new RASCONN[1]);

            var serviceLocator = new Mock<IServiceProvider>();

            var target = new TestableRasEnumConnectionsService(api.Object, deviceTypeFactory.Object, exceptionPolicy.Object, structFactory.Object, serviceLocator.Object)
            {
                OnCreateHandleFromPtrCallback = (ptr) => null
            };

            var ex = Assert.Throws<InvalidOperationException>(() => target.EnumerateConnections().Single());
            Assert.AreEqual("The handle was not created.", ex.Message);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheDeviceIsNull()
        {
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";
            var deviceName = "WAN";

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasEnumConnections(It.IsAny<RASCONN[]>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumConnectionsCallback(
                (RASCONN[] o1, ref int o2, ref int o3) =>
                {
                    o1[0].hrasconn = new IntPtr(1);
                    o1[0].szDeviceName = deviceName;
                    o1[0].szDeviceType = RASDT_Vpn;
                    o1[0].szEntryName = entryName;
                    o1[0].szPhonebook = phoneBookPath;

                    o2 = 1;
                    o3 = 1;

                    return SUCCESS;
                }));

            var deviceTypeFactory = new Mock<IDeviceTypeFactory>();
            deviceTypeFactory.Setup(o => o.Create(deviceName, RASDT_Vpn)).Returns((RasDevice)null);

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var structFactory = new Mock<IStructArrayFactory>();
            structFactory.Setup(o => o.CreateArray<RASCONN>(1, out It.Ref<int>.IsAny)).Returns(new RASCONN[1]);

            var serviceLocator = new Mock<IServiceProvider>();

            var target = new RasEnumConnectionsService(api.Object, deviceTypeFactory.Object, exceptionPolicy.Object, structFactory.Object, serviceLocator.Object);

            var ex = Assert.Throws<InvalidOperationException>(() => target.EnumerateConnections().Single());
            Assert.AreEqual("The device was not created.", ex.Message);
        }
    }
}