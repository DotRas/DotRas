using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Policies;

namespace DotRas.Internal.Infrastructure.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterPolicies(Container container)
        {
            container.Register<IExceptionPolicy>(typeof(DefaultExceptionPolicy));
            container.RegisterType(typeof(RasDialCallbackExceptionPolicy));
            container.RegisterType(typeof(RasGetConnectStatusExceptionPolicy));            
        }
    }
}