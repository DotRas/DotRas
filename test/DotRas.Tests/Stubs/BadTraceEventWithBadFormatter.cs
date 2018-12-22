using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Tests.Stubs
{
    [Serializable]
    [EventFormatter(typeof(BadFormatter))]
    public class BadTraceEventWithBadFormatter : TraceEvent
    {
    }
}