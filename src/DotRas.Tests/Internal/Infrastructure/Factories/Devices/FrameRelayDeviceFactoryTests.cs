using DotRas.Devices;
using DotRas.Internal.Infrastructure.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Infrastructure.Factories.Devices {
    [TestFixture]
    public class FrameRelayDeviceFactoryTests {
        [Test]
        public void ReturnADeviceInstance() {
            var target = new FrameRelayDeviceFactory();
            var result = target.Create("Test");

            Assert.Multiple(() => {
                Assert.That(result.Name, Is.EqualTo("Test"));
                Assert.That(result, Is.AssignableFrom<FrameRelay>());
            });
        }
    }
}
