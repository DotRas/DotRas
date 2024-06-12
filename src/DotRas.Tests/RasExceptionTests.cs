using NUnit.Framework;

namespace DotRas.Tests {
    [TestFixture]
    public class RasExceptionTests {
        [Test]
        public void InitializeTheExceptionWithAMessage() {
            var target = new RasException(623, "This is a test exception!");

            Assert.Multiple(() => {
                Assert.That(target.NativeErrorCode, Is.EqualTo(623));
                Assert.That(target.Message, Is.EqualTo("This is a test exception!"));
            });
        }
    }
}
