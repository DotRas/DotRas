using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Infrastructure.Primitives;
using System;

namespace DotRas.Internal.Infrastructure.IoC {
    internal static partial class ContainerBuilder {
        private static void RegisterThreading(Container container) {
            container.Register<IValueWaiter<IntPtr>>(typeof(ValueWaiter<IntPtr>));
            container.Register<IFileSystem>(typeof(FileSystem));
        }
    }
}
