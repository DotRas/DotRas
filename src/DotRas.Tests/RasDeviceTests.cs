using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using System;

namespace DotRas.Tests {
    [TestFixture]
    public class RasDeviceTests {
        private Mock<IServiceProvider> container;
        private Mock<IRasEnumDevices> rasEnumDevices;

        [SetUp]
        public void Setup() {
            rasEnumDevices = new Mock<IRasEnumDevices>();

            container = new Mock<IServiceProvider>();
            ServiceLocator.SetLocator(() => container.Object);
        }

        [TearDown]
        public void TearDown() => ServiceLocator.Reset();

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsNull() {
            var target = new TestDevice(null);
            Assert.That(target.Name, Is.EqualTo(null));
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsWhitespace() {
            var target = new TestDevice("            ");
            Assert.That(target.Name, Is.EqualTo("            "));
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsEmpty() {
            var target = new TestDevice(string.Empty);
            Assert.That(target.Name, Is.EqualTo(string.Empty));
        }

        [Test]
        public void EnumeratesTheDevicesCorrectly() {
            rasEnumDevices.Setup(o => o.EnumerateDevices()).Returns(new RasDevice[0]);

            container.Setup(o => o.GetService(typeof(IRasEnumDevices))).Returns(rasEnumDevices.Object);
            var result = RasDevice.EnumerateDevices();

            Assert.That(result, Is.Not.Null);
            container.Verify(o => o.GetService(typeof(IRasEnumDevices)));
            rasEnumDevices.Verify(o => o.EnumerateDevices());
        }
    }
}
