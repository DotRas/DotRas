using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using System;

namespace DotRas.Tests.Stubs {
    [Serializable]
    [EventFormatter(typeof(object))]
    public class BadTraceEvent : TraceEvent { }
}
