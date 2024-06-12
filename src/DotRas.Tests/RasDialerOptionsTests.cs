using NUnit.Framework;

namespace DotRas.Tests {
    [TestFixture]
    public class RasDialerOptionsTests {
        [Test]
        public void ReturnsInterfaceIndexAsExpected() {
            var target = new RasDialerOptions { InterfaceIndex = 1 };

            Assert.That(target.InterfaceIndex, Is.EqualTo(1));
        }
    }
}
