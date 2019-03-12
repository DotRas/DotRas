using System;
using DotRas.Internal.Abstractions.IoC;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Primitives;

namespace DotRas.Internal.IoC
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterThreading(ICompositionRegistry registry)
        {
            registry.RegisterCallback<IManualResetEvent>(
                c => new ManualResetEvent());

            registry.RegisterCallback<IValueWaiter<IntPtr>>(
                c => new ValueWaiter<IntPtr>(
                    c.GetRequiredService<IManualResetEvent>()));

            registry.RegisterCallback<IFileSystem>(
                c => new FileSystem());
        }
    }
}