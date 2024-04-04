using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Tests.Stubs;

[Serializable]
[EventFormatter(typeof(object))]
public class BadTraceEvent : TraceEvent
{
}