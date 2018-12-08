using System.ComponentModel.Design;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Primitives;
using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterThreading(IServiceContainer container)
        {
            container.AddService(typeof(IManualResetEvent),
                (c, _) => new ManualResetEvent());

            container.AddService(typeof(IValueWaiter<RasHandle>),
                (c, _) => new ValueWaiter<RasHandle>(
                    c.GetRequiredService<IManualResetEvent>()));

        }
    }
}