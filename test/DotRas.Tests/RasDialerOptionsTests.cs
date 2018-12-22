using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasDialerOptionsTests
    {
        [Test]
        public void ReturnsTrueForAllowUseStoredCredentials()
        {
            var target = new RasDialerOptions
            {
                AllowUseStoredCredentials = true
            };

            Assert.True(target.AllowUseStoredCredentials);
        }

        [Test]
        public void ReturnsInterfaceIndexAsExpected()
        {
            var target = new RasDialerOptions
            {
                InterfaceIndex = 1
            };

            Assert.AreEqual(1, target.InterfaceIndex);
        }
    }
}