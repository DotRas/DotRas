using DotRas.Devices;
using DotRas.Internal.Composition.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Composition.Factories.Devices
{
    [TestFixture]
    public class ParallelDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new ParallelDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Parallel>(result);
        }
    }
}