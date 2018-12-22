using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class Win32ErrorInformationTests
    {
        [Test]
        public void ReturnsTheErrorCodeAsExpected()
        {
            var errorCode = 1;
            var message = "test";

            var target = new Win32ErrorInformation(errorCode, message);
            Assert.AreEqual(errorCode, target.ErrorCode);
        }

        [Test]
        public void ReturnsTheMessageAsExpected()
        {
            var errorCode = 1;
            var message = "test";

            var target = new Win32ErrorInformation(errorCode, message);
            Assert.AreEqual(message, target.Message);
        }

        [Test]
        public void CanCreateAMockInstance()
        {
            var target = new Mock<Win32ErrorInformation>();
            var actual = target.Object;

            Assert.IsNotNull(actual);
        }
    }
}