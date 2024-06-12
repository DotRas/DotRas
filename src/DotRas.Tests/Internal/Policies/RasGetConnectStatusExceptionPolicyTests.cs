using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;
using Moq;
using NUnit.Framework;
using System;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Policies {
    [TestFixture]
    public class RasGetConnectStatusExceptionPolicyTests {
        [Test]
        public void ThrowsAnExceptionWhenGetErrorStringIsNull() => Assert.Throws<ArgumentNullException>(() => new RasGetConnectStatusExceptionPolicy(null));

        [Test]
        public void ShouldTranslateTheErrorCodeAsExpected() {
            var rasGetErrorString = new Mock<IRasGetErrorString>();
            rasGetErrorString.Setup(o => o.GetErrorString(ERROR_NO_CONNECTION)).Returns("No connection");

            var target = new RasGetConnectStatusExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(ERROR_INVALID_HANDLE);

            Assert.That(result, Is.InstanceOf<RasException>());

            var ex = (RasException)result;
            Assert.Multiple(() => {
                Assert.That(ex.NativeErrorCode, Is.EqualTo(ERROR_NO_CONNECTION));
                Assert.That(ex.Message, Is.EqualTo("No connection"));
            });

            Assert.That(target.TranslatedToNoConnection, Is.True);
        }
    }
}
