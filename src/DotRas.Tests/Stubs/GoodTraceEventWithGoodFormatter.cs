using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using System;

namespace DotRas.Tests.Stubs {
    [Serializable]
    [EventFormatter(typeof(GoodFormatter))]
    public class GoodTraceEventWithGoodFormatter : TraceEvent { }
}
