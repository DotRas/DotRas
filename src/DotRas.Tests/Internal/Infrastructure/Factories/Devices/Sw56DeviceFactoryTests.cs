using DotRas.Devices;
using DotRas.Internal.Infrastructure.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Infrastructure.Factories.Devices {
    [TestFixture]
    public class Sw56DeviceFactoryTests {
        [Test]
        public void ReturnADeviceInstance() {
            var target = new Sw56DeviceFactory();
            var result = target.Create("Test");

            Assert.Multiple(() => {
                Assert.That(result.Name, Is.EqualTo("Test"));
                Assert.That(result, Is.AssignableFrom<Sw56>());
            });
        }
    }
}
