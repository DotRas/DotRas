using System.Net;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionStatusTests
    {
        private Mock<RasDevice> device;

        [SetUp]
        public void Setup()
        {
            device = new Mock<RasDevice>();
        }

        [Test]
        public void DoesNotThrowAnErrorWithNullLocalEndPoint()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            IPAddress localEndpoint = null;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.Null(target.LocalEndPoint);
        }

        [Test]
        public void DoesNotThrowAnErrorWithNullRemoteEndPoint()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            IPAddress remoteEndpoint = null;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.Null(target.RemoteEndPoint);
        }

        [Test]
        public void ReturnsTheConnectionStateAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(connectionState, target.ConnectionState);
        }

        [Test]
        public void ReturnsTheDeviceAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(device.Object, target.Device);
        }

        [Test]
        public void ReturnsThePhoneNumberAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(phoneNumber, target.PhoneNumber);
        }

        [Test]
        public void ReturnsTheLocalEndPointAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(localEndpoint, target.LocalEndPoint);
        }

        [Test]
        public void ReturnsTheRemoteEndPointAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(remoteEndpoint, target.RemoteEndPoint);
        }

        [Test]
        public void ReturnsTheConnectionSubStateAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(connectionSubState, target.ConnectionSubState);
        }

        [Test]
        public void ReturnsNullErrorCodeAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.Null(target.ErrorCode);
        }

        [Test]
        public void ReturnsErrorCodeWithValueAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, 1, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(1, target.ErrorCode);
        }

    }
}