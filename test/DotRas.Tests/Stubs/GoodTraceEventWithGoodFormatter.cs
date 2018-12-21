using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Tests.Stubs
{
    [Serializable]
    [Formatter(typeof(GoodFormatter))]
    public class GoodTraceEventWithGoodFormatter : TraceEvent
    {
    }
}
