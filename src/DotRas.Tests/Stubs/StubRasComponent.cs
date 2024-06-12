using System;

namespace DotRas.Tests.Stubs {
    public class StubRasComponent : RasComponentBase {
        public event EventHandler<EventArgs> SomethingOccurred;

        public void RaiseSomethingOccurredEvent(EventArgs e) => RaiseEvent(SomethingOccurred, e);

        public void RaiseInternalErrorEvent(ErrorEventArgs e) => RaiseErrorEvent(e);
    }
}
