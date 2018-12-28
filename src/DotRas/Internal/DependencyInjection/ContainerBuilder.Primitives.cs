using System;
using System.ComponentModel.Design;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Primitives;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterThreading(IServiceContainer container)
        {
            container.AddService(typeof(IManualResetEvent),
                (c, _) => new ManualResetEvent());

            container.AddService(typeof(IValueWaiter<IntPtr>),
                (c, _) => new ValueWaiter<IntPtr>(
                    c.GetRequiredService<IManualResetEvent>()));

            container.AddService(typeof(IFileSystem),
                (c, _) => new FileSystem());
        }
    }
}