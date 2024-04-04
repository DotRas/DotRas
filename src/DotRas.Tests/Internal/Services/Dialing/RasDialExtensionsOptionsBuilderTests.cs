using DotRas.Internal.Services.Dialing;
using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services.Dialing;

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
    public void DoesNotAppendTheFlag()
    {
        var target = new RasDialExtensionsOptionsBuilder();
        target.AppendFlagIfTrue(false, RDEOPT.PausedStates);

        Assert.AreEqual(RDEOPT.None, target.Result);
    }

    [Test]
    public void AppendsTheFlag()
    {
        var target = new RasDialExtensionsOptionsBuilder();
        target.AppendFlagIfTrue(true, RDEOPT.PausedStates);

        Assert.AreEqual(RDEOPT.PausedStates, target.Result);
    }
}