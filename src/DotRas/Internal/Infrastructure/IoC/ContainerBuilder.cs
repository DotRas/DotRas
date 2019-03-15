using System;

namespace DotRas.Internal.Infrastructure.IoC
{
    internal static partial class ContainerBuilder
    {
        public static Container Build()
        {
            var container = new Container();

            container.Register<IServiceProvider>(() => container);

            RegisterDiagnostics(container);
            RegisterFactories(container);
            RegisterInternal(container);
            RegisterInterop(container);

            return container;
        }
    }
}