using DotRas.Internal.Abstractions.Services;

namespace DotRas.Tests.Stubs;

internal class TestableRasConnectionWatcher : RasConnectionWatcher
{
    public TestableRasConnectionWatcher(IRasConnectionNotification api)
        : base(api)
    {
    }

    public new void RaiseConnectedEvent(RasConnectionEventArgs e)
    {
        base.RaiseConnectedEvent(e);
    }

    public new void RaiseDisconnectedEvent(RasConnectionEventArgs e)
    {
        base.RaiseDisconnectedEvent(e);
    }
}