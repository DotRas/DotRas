using DotRas.Devices;
using DotRas.Internal.Composition.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Composition.Factories.Devices
{
    [TestFixture]
    public class GenericDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new GenericDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Generic>(result);
        }
    }
}