using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionOptionsTests
    {
        [Test]
        public void MustReturnTrueForAllUsers()
        {
            var target = new RasConnectionOptions(RASCF.AllUsers);

            Assert.True(target.AllUsers);
            Assert.False(target.GlobalCredentials);
            Assert.False(target.OwnerKnown);
            Assert.False(target.OwnerMatch);
        }

        [Test]
        public void MustReturnTrueForGlobalCreds()
        {
            var target = new RasConnectionOptions(RASCF.GlobalCreds);

            Assert.False(target.AllUsers);
            Assert.True(target.GlobalCredentials);
            Assert.False(target.OwnerKnown);
            Assert.False(target.OwnerMatch);
        }

        [Test]
        public void MustReturnTrueForOwnerKnown()
        {
            var target = new RasConnectionOptions(RASCF.OwnerKnown);

            Assert.False(target.AllUsers);
            Assert.False(target.GlobalCredentials);
            Assert.True(target.OwnerKnown);
            Assert.False(target.OwnerMatch);
        }

        [Test]
        public void MustReturnTrueForOwnerMatch()
        {
            var target = new RasConnectionOptions(RASCF.OwnerMatch);

            Assert.False(target.AllUsers);
            Assert.False(target.GlobalCredentials);
            Assert.False(target.OwnerKnown);
            Assert.True(target.OwnerMatch);
        }
    }
}