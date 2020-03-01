using System;
using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasDeviceTests
    {
        private Mock<IServiceProvider> container;
        private Mock<IRasEnumDevices> rasEnumDevices;

        [SetUp]
        public void Setup()
        {
            rasEnumDevices = new Mock<IRasEnumDevices>();

            container = new Mock<IServiceProvider>();
            ServiceLocator.Default = container.Object;
        }

        [TearDown]
        public void TearDown()
        {
            ServiceLocator.Clear();
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsNull()
        {
            var target = new TestDevice(null);
            Assert.AreEqual(null, target.Name);
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsWhitespace()
        {
            var target = new TestDevice("            ");
            Assert.AreEqual("            ", target.Name);
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsEmpty()
        {
            var target = new TestDevice(string.Empty);
            Assert.AreEqual(string.Empty, target.Name);
        }

        [Test]
        public void EnumeratesTheDevicesCorrectly()
        {
            rasEnumDevices.Setup(o => o.EnumerateDevices()).Returns(new RasDevice[0]);

            container.Setup(o => o.GetService(typeof(IRasEnumDevices))).Returns(rasEnumDevices.Object);
            var result = RasDevice.EnumerateDevices();

            Assert.IsNotNull(result);
            container.Verify(o => o.GetService(typeof(IRasEnumDevices)));
            rasEnumDevices.Verify(o => o.EnumerateDevices());
        }
    }
}