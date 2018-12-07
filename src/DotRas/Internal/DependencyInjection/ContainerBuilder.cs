using System.ComponentModel.Design;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        public static IServiceContainer Build()
        {
            var container = new ServiceContainer();

            RegisterDiagnostics(container);
            RegisterFactories(container);
            RegisterInternal(container);
            RegisterWin32(container);

            return container;
        }
    }
}