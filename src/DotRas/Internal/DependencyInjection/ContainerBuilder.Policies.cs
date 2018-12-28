using System.ComponentModel.Design;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterPolicies(IServiceContainer container)
        {
            container.AddService(typeof(DefaultExceptionPolicy),
                (c, _) => new DefaultExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));

            container.AddService(typeof(RasGetConnectStatusExceptionPolicy),
                (c, _) => new RasGetConnectStatusExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));
        }
    }
}