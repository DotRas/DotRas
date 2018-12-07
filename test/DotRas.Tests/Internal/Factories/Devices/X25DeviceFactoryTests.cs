using NUnit.Framework;
using DotRas.Devices;
using DotRas.Internal.Factories.Devices;

namespace DotRas.Tests.Internal.Factories.Devices
{
    [TestFixture]
    public class X25DeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new X25DeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<X25>(result);
        }
    }
}