using DotRas.Devices;
using DotRas.Internal.DependencyInjection.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.DependencyInjection.Factories.Devices
{
    [TestFixture]
    public class ModemDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new ModemDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Modem>(result);
        }
    }
}