using DotRas.Diagnostics;

namespace DotRas.Tests.Stubs
{
    public class GoodFormatter : IEventFormatter<GoodTraceEventWithGoodFormatter>
    {
        public string Format(GoodTraceEventWithGoodFormatter eventData)
        {
            return "Good";
        }
    }
}