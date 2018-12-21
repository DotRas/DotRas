using System.ComponentModel.Design;
using System.Diagnostics;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Tracing;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterDiagnostics(IServiceContainer container)
        {
            container.AddService(typeof(IEventLoggingPolicy), 
                (c, _) => new DefaultEventLoggingPolicy(
                    c.GetRequiredService<ILog>()));

            container.AddService(typeof(ILog), (c, _) =>
                Logger.Current ?? new TraceLog(
                    c.GetRequiredService<IEventFormatterAdapter>(),
                    c.GetRequiredService<IConverter<EventLevel, TraceEventType>>()));

            container.AddService(typeof(IEventFormatterFactory), (c, _) => new ConventionBasedEventFormatterFactory());

            container.AddService(typeof(IEventFormatterAdapter), 
                (c, _) => new EventFormatterAdapter(
                    c.GetRequiredService<IEventFormatterFactory>()));

            container.AddService(typeof(IConverter<EventLevel, TraceEventType>), (c, _) => new EventLevelConverter());
        }
    }
}