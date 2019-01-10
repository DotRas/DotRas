using DotRas.Internal.Abstractions.Services;

namespace DotRas.Tests.Stubs
{
    internal class TestableRasConnectionWatcher : RasConnectionWatcher
    {
        public TestableRasConnectionWatcher(IRasConnectionNotification api)
            : base(api)
        {
        }
    }
}