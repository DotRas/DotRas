using System;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Primitives;

namespace DotRas.Internal.Infrastructure.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterThreading(Container container)
        {
            container.Register<IManualResetEvent>(typeof(ManualResetEvent));
            container.Register<IValueWaiter<IntPtr>>(typeof(ValueWaiter<IntPtr>));
            container.Register<IFileSystem>(typeof(FileSystem));
        }
    }
}