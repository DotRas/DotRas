using DotRas.Devices;
using DotRas.Internal.DependencyInjection.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.DependencyInjection.Factories.Devices
{
    [TestFixture]
    public class SerialDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new SerialDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Serial>(result);
        }
    }
}