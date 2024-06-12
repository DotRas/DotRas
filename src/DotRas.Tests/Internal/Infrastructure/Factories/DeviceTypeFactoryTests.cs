using DotRas.Devices;
using DotRas.Internal.Infrastructure.Factories;
using Moq;
using NUnit.Framework;
using System;

namespace DotRas.Tests.Internal.Infrastructure.Factories {
    [TestFixture]
    public class DeviceTypeFactoryTests {
        [Test]
        public void ThrowsAnExceptionWhenTheServiceLocatorIsNull() =>
            Assert.Throws<ArgumentNullException>(() => {
                _ = new DeviceTypeFactory(null);
            });

        [Test]
        public void ReturnsAnUnknownDeviceWhenNotExpected() {
            var serviceLocator = new Mock<IServiceProvider>();

            var target = new DeviceTypeFactory(serviceLocator.Object);
            var result = target.Create("unknown-name", "unknown-device");

            Assert.That(result, Is.InstanceOf<Unknown>());

            var device = (Unknown)result;
            Assert.Multiple(() => {
                Assert.That(device.Name, Is.EqualTo("unknown-name"));
                Assert.That(device.DeviceType, Is.EqualTo("unknown-device"));
            });
        }
    }
}
