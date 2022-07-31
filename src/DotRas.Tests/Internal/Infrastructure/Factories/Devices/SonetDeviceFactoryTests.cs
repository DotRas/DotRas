using DotRas.Devices;
using DotRas.Internal.Infrastructure.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Infrastructure.Factories.Devices
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