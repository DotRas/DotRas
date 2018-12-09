using DotRas.Devices;
using DotRas.Internal.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Factories.Devices
{
    [TestFixture]
    public class SonetDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new SonetDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Sonet>(result);
        }
    }
}