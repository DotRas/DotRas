using System;
using DotRas.Internal.Interop;
using DotRas.Internal.Providers;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Providers
{
    [TestFixture]
    public class PhoneBookEntryValidatorTests
    {
        [Test]
        public void ThrowsAnExceptionWhenApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PhoneBookEntryValidator(null));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheEntryNameIsNull()
        {
            var api = new Mock<IRasApi32>();

            var target = new PhoneBookEntryValidator(api.Object);
            Assert.Throws<ArgumentNullException>(() => target.VerifyEntryExists(null, "PATH"));
        }

        [Test]
        public void DoesNotThrowAnExceptionWhenThePhoneBookPathIsNull()
        {
            var api = new Mock<IRasApi32>();

            var target = new PhoneBookEntryValidator(api.Object);
            Assert.DoesNotThrow(() => target.VerifyEntryExists("ENTRY", null));
        }

        [Test]
        public void ReturnsTrueWhenTheEntryExistsAsExpected()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasValidateEntryName("PATH", "ENTRY")).Returns(ERROR_ALREADY_EXISTS);

            var target = new PhoneBookEntryValidator(api.Object);
            var result = target.VerifyEntryExists("ENTRY", "PATH");

            Assert.True(result);
        }

        [Test]
        public void ReturnsFalseWhenTheEntryDoesNotExistAsExpected()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasValidateEntryName("PATH", "ENTRY")).Returns(SUCCESS);

            var target = new PhoneBookEntryValidator(api.Object);
            var result = target.VerifyEntryExists("ENTRY", "PATH");

            Assert.False(result);
        }
    }
}