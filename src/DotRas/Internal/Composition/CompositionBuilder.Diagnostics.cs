using DotRas.Diagnostics;
using DotRas.Diagnostics.Tracing;
using DotRas.Internal.Abstractions.Composition;

namespace DotRas.Internal.Composition
{
    internal static partial class CompositionBuilder
    {
        private static void RegisterDiagnostics(ICompositionRegistry registry)
        {
            registry.RegisterCallback<IEventLoggingPolicy>(
                    c => new DefaultEventLoggingPolicy(
                        c.GetRequiredService<ILog>()));

            registry.RegisterCallback<ILog>(
                c => new TraceLog(
                    c.GetRequiredService<IEventFormatterAdapter>(),
                    c.GetRequiredService<IEventLevelConverter>()));

            registry.RegisterCallback<IEventFormatterFactory>(
                c => new ConventionBasedEventFormatterFactory());

            registry.RegisterCallback<IEventFormatterAdapter>(
                c => new EventFormatterAdapter(
                    c.GetRequiredService<IEventFormatterFactory>()));

            registry.RegisterCallback<IEventLevelConverter>(
                c => new EventLevelConverter());
        }
    }
}