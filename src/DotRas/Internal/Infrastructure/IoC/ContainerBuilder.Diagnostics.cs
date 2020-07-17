using DotRas.Diagnostics;
using DotRas.Diagnostics.Tracing;

namespace DotRas.Internal.Infrastructure.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterDiagnostics(Container container)
        {
            container.Register<IEventLoggingPolicy>(typeof(DefaultEventLoggingPolicy));
            container.RegisterType(typeof(TraceLogger)).AsSingleton();

            // Default the logging implementation to attempt to find something by the dependency resolver
            // prior to using the default implementation.
            container.Register(() => 
                LoggerLocator.Current ?? container.GetRequiredService<TraceLogger>());

            container.Register<IEventFormatterFactory>(typeof(ConventionBasedEventFormatterFactory));
            container.Register<IEventFormatterAdapter>(typeof(EventFormatterAdapter));
            container.Register<IEventLevelConverter>(typeof(EventLevelConverter));
        }
    }
}