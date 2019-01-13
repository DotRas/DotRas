using DotRas.Devices;
using DotRas.Internal.Composition.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Composition.Factories.Devices
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