using System.ComponentModel.Design;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterPolicies(IServiceContainer container)
        {
            container.AddService(typeof(IExceptionPolicy),
                (c, _) => new DefaultExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));

            container.AddService(typeof(RasDialCallbackExceptionPolicy),
                (c, _) => new RasDialCallbackExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));
        }
    }
}