using System;
using System.ComponentModel.Design;
using DotRas.Diagnostics;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Policies;
using DotRas.Internal.Services;
using DotRas.Internal.Services.Connections;
using DotRas.Internal.Services.Dialing;
using DotRas.Internal.Services.ErrorHandling;
using DotRas.Internal.Services.PhoneBooks;
using DotRas.Internal.Services.Security;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterInternal(IServiceContainer container)
        {
            RegisterPolicies(container);
            RegisterThreading(container);

            container.AddService(typeof(IAllocateLocallyUniqueId),
                (c, _) => new AllocateLocallyUniqueIdService(
                    c.GetRequiredService<IAdvApi32>()));

            container.AddService(typeof(IRasEnumConnections),
                (c, _) => new RasEnumConnectionsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IDeviceTypeFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>(),
                    c.GetRequiredService<IStructArrayFactory>(),
                    c));

            container.AddService(typeof(IIPAddressConverter),
                (c, _) => new IPAddressConversionService());

            container.AddService(typeof(IMarshaller),
                (c, _) => new MarshallerLoggingAdvice(
                    new MarshallingService(),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            container.AddService(typeof(IPhoneBookEntryValidator),
                (c, _) => new PhoneBookEntryNameValidationService(
                    c.GetRequiredService<IRasApi32>()));

            container.AddService(typeof(IRasClearConnectionStatistics),
                (c, _) => new RasClearConnectionStatisticsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            container.AddService(typeof(IRasHangUp),
                (c, _) => new RasHangUpService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            container.AddService(typeof(IRasGetConnectionStatistics),
                (c, _) => new RasGetConnectionStatisticsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            container.AddService(typeof(IRasGetConnectStatus),
                (c, _) => new RasGetConnectStatusService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IIPAddressConverter>(),
                    c.GetRequiredService<RasGetConnectStatusExceptionPolicy>(),
                    c.GetRequiredService<IDeviceTypeFactory>()));

            container.AddService(typeof(IRasGetErrorString),
                (c, _) => new RasGetErrorStringService(
                    c.GetRequiredService<IRasApi32>()));

            container.AddService(typeof(IRasGetCredentials),
                (c, _) => new RasGetCredentialsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            container.AddService(typeof(IRasDialExtensionsBuilder),
                (c, _) => new RasDialExtensionsBuilder(
                    c.GetRequiredService<IStructFactory>()));

            container.AddService(typeof(IRasDialParamsBuilder),
                (c, _) => new RasDialParamsBuilder(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            container.AddService(typeof(IRasDial),
                (c, _) => new RasDialService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IRasHangUp>(),
                    c.GetRequiredService<IRasDialExtensionsBuilder>(),
                    c.GetRequiredService<IRasDialParamsBuilder>(),
                    c.GetRequiredService<DefaultExceptionPolicy>(),
                    c.GetRequiredService<IRasDialCallbackHandler>(),
                    c.GetRequiredService<ITaskCompletionSourceFactory>(),
                    c.GetRequiredService<ITaskCancellationSourceFactory>()));

            container.AddService(typeof(IRasDialCallbackHandler),
                (c, _) => new RasDialCallbackHandlerLoggingAdvice(
                    new DefaultRasDialCallbackHandler(
                        c.GetRequiredService<IRasHangUp>(),
                        c.GetRequiredService<IRasEnumConnections>(),
                        c.GetRequiredService<RasDialCallbackExceptionPolicy>(),
                        c.GetRequiredService<IValueWaiter<IntPtr>>(),
                        c.GetRequiredService<ITaskCancellationSourceFactory>()),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            container.AddService(typeof(IWin32FormatMessage),
                (c, _) => new Win32FormatMessageService(
                    c.GetRequiredService<IKernel32>(),
                    c.GetRequiredService<IMarshaller>()));
        }
    }
}