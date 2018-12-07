using NUnit.Framework;
using DotRas.Devices;
using DotRas.Internal.Factories.Devices;

namespace DotRas.Tests.Internal.Factories.Devices
{
    [TestFixture]
    public class AtmDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new AtmDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Atm>(result);
        }
    }
}