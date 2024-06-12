using Moq;
using NUnit.Framework;
using System.Net;

namespace DotRas.Tests {
    [TestFixture]
    public class RasConnectionStatusTests {
        private Mock<RasDevice> device;

        [SetUp]
        public void Setup() => device = new Mock<RasDevice>();

        [Test]
        public void DoesNotThrowAnErrorWithNullLocalEndPoint() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            IPAddress localEndpoint = null;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.LocalEndPoint, Is.Null);
        }

        [Test]
        public void DoesNotThrowAnErrorWithNullRemoteEndPoint() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            IPAddress remoteEndpoint = null;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.RemoteEndPoint, Is.Null);
        }

        [Test]
        public void ReturnsTheConnectionStateAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.ConnectionState, Is.EqualTo(connectionState));
        }

        [Test]
        public void ReturnsTheDeviceAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.Device, Is.EqualTo(device.Object));
        }

        [Test]
        public void ReturnsThePhoneNumberAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.PhoneNumber, Is.EqualTo(phoneNumber));
        }

        [Test]
        public void ReturnsTheLocalEndPointAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.LocalEndPoint, Is.EqualTo(localEndpoint));
        }

        [Test]
        public void ReturnsTheRemoteEndPointAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.RemoteEndPoint, Is.EqualTo(remoteEndpoint));
        }

        [Test]
        public void ReturnsTheConnectionSubStateAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.ConnectionSubState, Is.EqualTo(connectionSubState));
        }

        [Test]
        public void ReturnsNullErrorCodeAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, null, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.ErrorCode, Is.Null);
        }

        [Test]
        public void ReturnsErrorCodeWithValueAsExpected() {
            var connectionState = RasConnectionState.Connected;
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, 1, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.That(target.ErrorCode, Is.EqualTo(1));
        }
    }
}
