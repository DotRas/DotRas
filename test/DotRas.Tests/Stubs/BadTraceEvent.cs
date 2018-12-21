using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Tests.Stubs
{
    [Serializable]
    [Formatter(typeof(object))]
    public class BadTraceEvent : TraceEvent
    {
    }
}