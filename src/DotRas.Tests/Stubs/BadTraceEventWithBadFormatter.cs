using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using System;

namespace DotRas.Tests.Stubs {
    [Serializable]
    [EventFormatter(typeof(BadFormatter))]
    public class BadTraceEventWithBadFormatter : TraceEvent { }
}
