using System.ComponentModel.Design;
using DotRas.Diagnostics;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Providers;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Internal.DependencyInjection.Advice;
using DotRas.Internal.Providers;
using DotRas.Internal.Services;
using DotRas.Internal.Services.Connections;
using DotRas.Win32;
using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterInternal(IServiceContainer container)
        {
            RegisterPolicies(container);
            RegisterThreading(container);

            container.AddService(typeof(IRasEnumConnections),
                (c, _) => new RasEnumConnections(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IDeviceTypeFactory>(),
                    c.GetRequiredService<IExceptionPolicy>(),
                    c.GetRequiredService<IStructArrayFactory>()));

            container.AddService(typeof(IStructMarshaller),
                (c, _) => new StructMarshallerLoggingAdvice(
                    new StructMarshaller(),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            container.AddService(typeof(IRasHangUp),
                (c, _) => new RasHangUp(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IExceptionPolicy>()));

            container.AddService(typeof(IRasGetConnectStatus),
                (c, _) => new RasGetConnectStatus(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IExceptionPolicy>(),
                    c.GetRequiredService<IDeviceTypeFactory>()));

            container.AddService(typeof(IRasGetErrorString),
                (c, _) => new RasGetErrorString(
                    c.GetRequiredService<IRasApi32>()));

            container.AddService(typeof(IRasDial),
                (c, _) => new RasDial(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IExceptionPolicy>(),
                    c.GetRequiredService<IRasDialCallbackHandler>()));

            container.AddService(typeof(IRasDialCallbackHandler),
                (c, _) => new RasDialCallbackHandlerLoggingAdvice(
                    new DefaultRasDialCallbackHandler(
                        c.GetRequiredService<IRasHangUp>(),
                        c.GetRequiredService<IRasEnumConnections>(),
                        c.GetRequiredService<IExceptionPolicy>(),
                        c.GetRequiredService<IValueWaiter<RasHandle>>(),
                        c.GetRequiredService<ITaskCancellationSourceFactory>()),
                    c.GetRequiredService<IEventLoggingPolicy>()));
        }
    }
}