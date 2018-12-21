using DotRas.Diagnostics;

namespace DotRas.Tests.Stubs
{
    public class GoodFormatter : IFormatter<GoodTraceEventWithGoodFormatter>
    {
        public string Format(GoodTraceEventWithGoodFormatter value)
        {
            return "Good";
        }
    }
}