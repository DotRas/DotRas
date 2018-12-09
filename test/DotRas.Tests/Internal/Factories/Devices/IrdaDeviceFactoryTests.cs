using DotRas.Devices;
using DotRas.Internal.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Factories.Devices
{
    public class IrdaDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new IrdaDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Irda>(result);
        }
    }
}