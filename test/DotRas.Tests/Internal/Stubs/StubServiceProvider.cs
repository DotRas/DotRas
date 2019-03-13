using System;

namespace DotRas.Tests.Internal.Stubs
{
    public class StubServiceProvider : IServiceProvider
    {
        private readonly object result;

        public StubServiceProvider(object result)
        {
            this.result = result;
        }

        public object GetService(Type serviceType)
        {
            return result;
        }
    }
}