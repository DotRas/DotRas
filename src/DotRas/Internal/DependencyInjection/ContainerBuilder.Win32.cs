using System.ComponentModel.Design;
using DotRas.Diagnostics;
using DotRas.Internal.DependencyInjection.Advice;
using DotRas.Win32;
using DotRas.Win32.Interop;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterWin32(IServiceContainer container)
        {
            container.AddService(typeof(IRasApi32), (c, _) => new RasApi32LoggingAdvice(
                new RasApi32(),
                c.GetRequiredService<IEventLoggingPolicy>()));
        }
    }
}