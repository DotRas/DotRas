using System;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionExtensionsTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheConnectionIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => RasConnectionExtensions.IsOwner(null));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheConnectionIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => RasConnectionExtensions.IsNotOwner(null));
        }

        [Test]
        public void ReturnTrueWhenTheCurrentUserOwnsTheConnection()
        {
            var target = new Mock<RasConnection>();
            target.Setup(o => o.Options.IsOwnerCurrentUser).Returns(true);
            target.Setup(o => o.Options.IsOwnerKnown).Returns(true);

            var result = target.Object.IsOwner();
            Assert.True(result);
        }

        [Test]
        public void ReturnFalseWhenTheCurrentUserDoesNotOwnTheConnection()
        {
            var target = new Mock<RasConnection>();
            target.Setup(o => o.Options.IsOwnerCurrentUser).Returns(false);
            target.Setup(o => o.Options.IsOwnerKnown).Returns(true);

            var result = target.Object.IsOwner();
            Assert.False(result);
        }

        [Test]
        public void ReturnTrueWhenTheCurrentUserDoesNotOwnTheConnection()
        {
            var target = new Mock<RasConnection>();
            target.Setup(o => o.Options.IsOwnerCurrentUser).Returns(false);
            target.Setup(o => o.Options.IsOwnerKnown).Returns(true);

            var result = target.Object.IsNotOwner();
            Assert.True(result);
        }

        [Test]
        public void ReturnFalseWhenTheCurrentUserDoesOwnTheConnection()
        {
            var target = new Mock<RasConnection>();
            target.Setup(o => o.Options.IsOwnerCurrentUser).Returns(true);
            target.Setup(o => o.Options.IsOwnerKnown).Returns(true);

            var result = target.Object.IsNotOwner();
            Assert.False(result);
        }
    }
}