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

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber);
            Assert.AreEqual(connectionState, target.ConnectionState);
        }

        [Test]
        public void ReturnsTheErrorInformationAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber);
            Assert.AreEqual(errorInformation.Object, target.ErrorInformation);
        }

        [Test]
        public void ReturnsTheDeviceAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber);
            Assert.AreEqual(device.Object, target.Device);
        }

        [Test]
        public void ReturnsThePhoneNumberAsExpected()
        {
            var connectionState = RasConnectionState.Connected;
            var errorInformation = new Mock<Win32ErrorInformation>();
            var device = new Mock<RasDevice>();
            var phoneNumber = "12345";

            var target = new RasConnectionStatus(connectionState, errorInformation.Object, device.Object, phoneNumber);
            Assert.AreEqual(phoneNumber, target.PhoneNumber);
        }
    }
}