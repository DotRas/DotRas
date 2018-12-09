using DotRas.Devices;
using DotRas.Internal.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Factories.Devices
{
    [TestFixture]
    public class VpnDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new VpnDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Vpn>(result);
        }
    }
}