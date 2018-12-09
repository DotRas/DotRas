using DotRas.Devices;
using DotRas.Internal.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Factories.Devices
{
    [TestFixture]
    public class Sw56DeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new Sw56DeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Sw56>(result);
        }
    }
}