using DotRas.Diagnostics;
using DotRas.Internal.Abstractions.Composition;
using DotRas.Internal.Composition.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Interop.Primitives;

namespace DotRas.Internal.Composition
{
    internal static partial class CompositionBuilder
    {
        private static void RegisterInterop(ICompositionRegistry registry)
        {
            registry.RegisterCallback<IAdvApi32>(
                c => new AdvApi32LoggingAdvice(
                    new AdvApi32(),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            registry.RegisterCallback<IKernel32>(
                c => new Kernel32LoggingAdvice(
                    new Kernel32(),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            registry.RegisterCallback<IRasApi32>(
                c => new RasApi32LoggingAdvice(
                    new RasApi32(),
                    c.GetRequiredService<IEventLoggingPolicy>()));
        }
    }
}