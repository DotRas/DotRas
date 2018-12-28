using System;
using System.ComponentModel;
using DotRas.Internal.Policies;
using NUnit.Framework;
using Moq;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.EapHostError;

namespace DotRas.Tests.Internal.Policies
{
    [TestFixture]
    public class RasDialCallbackExceptionPolicyTests
    {
        [Test]
        public void ThrowsAnExceptionWhenGetErrorStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasDialCallbackExceptionPolicy(null));
        }

        [Test]
        public void ShouldReturnTheMessageFromEapAsExpected()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();
            
            var target = new RasDialCallbackExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(EAP_E_USER_NAME_PASSWORD_REJECTED) as EapException;

            Assert.IsNotNull(result);
            Assert.AreEqual("Authenticator rejected user credentials for authentication.", result.Message);
        }

        [Test]
        public void PassesTheErrorOnWhenNotKnown()
        {
            var rasGetErrorString = new Mock<IRasGetErrorString>();

            var target = new RasDialCallbackExceptionPolicy(rasGetErrorString.Object);
            var result = target.Create(-1);

            Assert.IsInstanceOf<Win32Exception>(result);
        }
    }
}