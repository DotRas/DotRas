using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests {
    [TestFixture]
    public class RasConnectionOptionsTests {
        [Test]
        public void MustReturnTrueForAllUsers() {
            var target = new RasConnectionOptions(RASCF.AllUsers);

            Assert.Multiple(() => {
                Assert.That(target.AvailableToAllUsers, Is.True);
                Assert.That(target.UsingDefaultCredentials, Is.False);
                Assert.That(target.IsOwnerKnown, Is.False);
                Assert.That(target.IsOwnerCurrentUser, Is.False);
            });
        }

        [Test]
        public void MustReturnTrueForGlobalCreds() {
            var target = new RasConnectionOptions(RASCF.GlobalCreds);

            Assert.Multiple(() => {
                Assert.That(target.AvailableToAllUsers, Is.False);
                Assert.That(target.UsingDefaultCredentials, Is.True);
                Assert.That(target.IsOwnerKnown, Is.False);
                Assert.That(target.IsOwnerCurrentUser, Is.False);
            });
        }

        [Test]
        public void MustReturnTrueForOwnerKnown() {
            var target = new RasConnectionOptions(RASCF.OwnerKnown);

            Assert.Multiple(() => {
                Assert.That(target.AvailableToAllUsers, Is.False);
                Assert.That(target.UsingDefaultCredentials, Is.False);
                Assert.That(target.IsOwnerKnown, Is.True);
                Assert.That(target.IsOwnerCurrentUser, Is.False);
            });
        }

        [Test]
        public void MustReturnTrueForOwnerMatch() {
            var target = new RasConnectionOptions(RASCF.OwnerMatch);

            Assert.Multiple(() => {
                Assert.That(target.AvailableToAllUsers, Is.False);
                Assert.That(target.UsingDefaultCredentials, Is.False);
                Assert.That(target.IsOwnerKnown, Is.False);
                Assert.That(target.IsOwnerCurrentUser, Is.True);
            });
        }
    }
}
