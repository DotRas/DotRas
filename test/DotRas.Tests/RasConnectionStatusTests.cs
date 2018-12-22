using System.Net;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionStatusTests
    {
        [Test]
        public void ReturnsTheConnectionStateAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(connectionState, target.ConnectionState);
        }

        [Test]
        public void ReturnsTheErrorInformationAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(errorInformation.Object, target.ErrorInformation);
        }

        [Test]
        public void ReturnsTheDeviceAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(device.Object, target.Device);
        }

        [Test]
        public void ReturnsThePhoneNumberAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(phoneNumber, target.PhoneNumber);
        }

        [Test]
        public void ReturnsTheLocalEndPointAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(localEndpoint, target.LocalEndPoint);
        }

        [Test]
        public void ReturnsTheRemoteEndPointAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(remoteEndpoint, target.RemoteEndPoint);
        }

        [Test]
        public void ReturnsTheConnectionSubStateAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";
            var localEndpoint = IPAddress.Loopback;
            var remoteEndpoint = IPAddress.Any;
            var connectionSubState = RasConnectionSubState.None;

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber, localEndpoint, remoteEndpoint, connectionSubState);
            Assert.AreEqual(connectionSubState, target.ConnectionSubState);
        }
    }
}