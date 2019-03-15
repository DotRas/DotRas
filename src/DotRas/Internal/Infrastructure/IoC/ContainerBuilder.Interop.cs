using DotRas.Diagnostics;
using DotRas.Internal.Infrastructure.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Interop.Primitives;

namespace DotRas.Internal.Infrastructure.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterInterop(Container container)
        {
            container.Register<IAdvApi32>(() =>
                new AdvApi32LoggingAdvice(
                    new AdvApi32(),
                    container.GetRequiredService<IEventLoggingPolicy>()));

            container.Register<IKernel32>(() =>
                new Kernel32LoggingAdvice(
                    new Kernel32(),
                    container.GetRequiredService<IEventLoggingPolicy>()));

            container.Register<IRasApi32>(() =>
                new RasApi32LoggingAdvice(
                    new RasApi32(),
                    container.GetRequiredService<IEventLoggingPolicy>()));
        }
    }
}