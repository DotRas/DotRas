using System.ComponentModel.Design;
using DotRas.Diagnostics;
using DotRas.Internal.DependencyInjection.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Interop.Primitives;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterWin32(IServiceContainer container)
        {
            container.AddService(typeof(IRasApi32), (c, _) => new RasApi32LoggingAdvice(
                new RasApi32(),
                c.GetRequiredService<IEventLoggingPolicy>()));

            container.AddService(typeof(IAdvApi32), (c, _) => new AdvApi32LoggingAdvice(
                new AdvApi32(), 
                c.GetRequiredService<IEventLoggingPolicy>()));
        }
    }
}