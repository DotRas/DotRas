using DotRas.Internal.Abstractions.IoC;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;

namespace DotRas.Internal.IoC
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterPolicies(ICompositionRegistry registry)
        {
            registry.RegisterCallback(
                c => new DefaultExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));

            registry.RegisterCallback(
                c => new RasDialCallbackExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));

            registry.RegisterCallback(
                c => new RasGetConnectStatusExceptionPolicy(
                    c.GetRequiredService<IRasGetErrorString>()));
        }
    }
}