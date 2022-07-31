using DotRas.Diagnostics;

namespace DotRas.Tests.Stubs
{
    public class BadFormatter : IEventFormatter<BadTraceEventWithBadFormatter>
    {
        private readonly string result;

        public BadFormatter(string result)
        {
            this.result = result;
        }

        public string Format(BadTraceEventWithBadFormatter eventData)
        {
            return result;
        }
    }
}