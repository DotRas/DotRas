﻿using DotRas.Internal.Abstractions.DependencyInjection;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Policies;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class CompositionBuilder
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