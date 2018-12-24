using DotRas.Internal.Services.Dialing;
using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services.Dialing
{
    [TestFixture]
    public class RasDialExtensionsOptionsBuilderTests
    {
        [Test]
        public void InitializesWithNoneAsResult()
        {
            var target = new RasDialExtensionsOptionsBuilder();
            Assert.AreEqual(RDEOPT.None, target.Result);
        }

        [Test]
        public void ResultReturnsAsExpectedForOneFlag()
        {
            var target = new RasDialExtensionsOptionsBuilder();
            target.AppendFlagIfTrue(true, RDEOPT.CustomDial);

            Assert.AreEqual(RDEOPT.CustomDial, target.Result);
        }

        [Test]
        public void ResultReturnsAsExpectedForTwoFlags()
        {
            var target = new RasDialExtensionsOptionsBuilder();
            target.AppendFlagIfTrue(true, RDEOPT.CustomDial);
            target.AppendFlagIfTrue(true, RDEOPT.PauseOnScript);

            Assert.AreEqual((RDEOPT.CustomDial | RDEOPT.PauseOnScript), target.Result);
        }

        [Test]
        public void ResultReturnsAsExpectedWhenOneFlagIsFalse()
        {
            var target = new RasDialExtensionsOptionsBuilder();
            target.AppendFlagIfTrue(true, RDEOPT.CustomDial);
            target.AppendFlagIfTrue(false, RDEOPT.PauseOnScript);

            Assert.AreEqual(RDEOPT.CustomDial, target.Result);
        }
    }
}