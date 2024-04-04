using DotRas.Internal.Abstractions.Services;

namespace DotRas.Tests.Stubs;

internal class TestableRasDialer : RasDialer
{
    public TestableRasDialer(IRasDial api)
        : base(api)
    {
    }

    public new void RaiseStateChangedEvent(StateChangedEventArgs e)
    {
        base.RaiseStateChangedEvent(e);
    }
}