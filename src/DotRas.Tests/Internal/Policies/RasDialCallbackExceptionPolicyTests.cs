using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel;
using static DotRas.Internal.Interop.EapHostError;

namespace DotRas.Tests.Internal.Policies {
    [TestFixture]
    public class RasDialCallbackExceptionPolicyTests {
        [Test]
        public void ThrowsAnExceptionWhenGetErrorStringIsNull() => Assert.Throws<ArgumentNullException>(() => new RasDialCallbackExceptionPolicy(null));

        [Test]
        public void ShouldReturnTheMessageFromEapAsExpected() {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var target = new RasDialCallbackExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(EAP_E_USER_NAME_PASSWORD_REJECTED) as EapException;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Authenticator rejected user credentials for authentication."));
        }

        [Test]
        public void PassesTheErrorOnWhenNotKnown() {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var target = new RasDialCallbackExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(-1);

            Assert.That(result, Is.InstanceOf<Win32Exception>());
        }
    }
}
