using DotRas.Diagnostics;
using DotRas.Diagnostics.Tracing;

namespace DotRas.Internal.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterDiagnostics(Container container)
        {
            container.Register<IEventLoggingPolicy>(typeof(DefaultEventLoggingPolicy));
            container.Register<ILog>(typeof(TraceLog));
            container.Register<IEventFormatterFactory>(typeof(ConventionBasedEventFormatterFactory));
            container.Register<IEventFormatterAdapter>(typeof(EventFormatterAdapter));
            container.Register<IEventLevelConverter>(typeof(EventLevelConverter));
        }
    }
}