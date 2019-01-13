using System;
using DotRas.Diagnostics;
using DotRas.Internal.Abstractions.Composition;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Composition.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Policies;
using DotRas.Internal.Services;
using DotRas.Internal.Services.Connections;
using DotRas.Internal.Services.Dialing;
using DotRas.Internal.Services.ErrorHandling;
using DotRas.Internal.Services.PhoneBooks;
using DotRas.Internal.Services.Security;

namespace DotRas.Internal.Composition
{
    internal static partial class CompositionBuilder
    {
        private static void RegisterInternal(ICompositionRegistry registry)
        {
            RegisterPolicies(registry);
            RegisterThreading(registry);

            registry.RegisterCallback<IAllocateLocallyUniqueId>(
                c => new AllocateLocallyUniqueIdService(
                    c.GetRequiredService<IAdvApi32>()));

            registry.RegisterCallback<IRasEnumConnections>(
                c => new RasEnumConnectionsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IDeviceTypeFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>(),
                    c.GetRequiredService<IStructArrayFactory>(),
                    c));

            registry.RegisterCallback<IIPAddressConverter>(
                c => new IPAddressConversionService());

            registry.RegisterCallback<IMarshaller>(
                c => new MarshallerLoggingAdvice(
                    new MarshallingService(),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            registry.RegisterCallback<IPhoneBookEntryValidator>(
                c => new PhoneBookEntryNameValidationService(
                    c.GetRequiredService<IRasApi32>()));

            registry.RegisterCallback<IRasClearConnectionStatistics>(
                c => new RasClearConnectionStatisticsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            registry.RegisterCallback<IRasConnectionNotification>(
                c => new RasConnectionNotificationService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IRasConnectionNotificationCallbackHandler>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            registry.RegisterCallback<IRasConnectionNotificationCallbackHandler>(
                c => new RasConnectionNotificationCallbackHandler(
                    c.GetRequiredService<IRasEnumConnections>()));

            registry.RegisterCallback<IRasHangUp>(
                c => new RasHangUpService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            registry.RegisterCallback<IRasGetConnectionStatistics>(
                c => new RasGetConnectionStatisticsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            registry.RegisterCallback<IRasGetConnectStatus>(
                c => new RasGetConnectStatusService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IIPAddressConverter>(),
                    c.GetRequiredService<RasGetConnectStatusExceptionPolicy>(),
                    c.GetRequiredService<IDeviceTypeFactory>()));

            registry.RegisterCallback<IRasGetErrorString>(
                c => new RasGetErrorStringService(
                    c.GetRequiredService<IRasApi32>()));

            registry.RegisterCallback<IRasGetCredentials>(
                c => new RasGetCredentialsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            registry.RegisterCallback<IRasDialExtensionsBuilder>(
                c => new RasDialExtensionsBuilder(
                    c.GetRequiredService<IStructFactory>()));

            registry.RegisterCallback<IRasDialParamsBuilder>(
                c => new RasDialParamsBuilder(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<DefaultExceptionPolicy>()));

            registry.RegisterCallback<IRasDial>(
                c => new RasDialService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IRasHangUp>(),
                    c.GetRequiredService<IRasDialExtensionsBuilder>(),
                    c.GetRequiredService<IRasDialParamsBuilder>(),
                    c.GetRequiredService<DefaultExceptionPolicy>(),
                    c.GetRequiredService<IRasDialCallbackHandler>(),
                    c.GetRequiredService<ITaskCompletionSourceFactory>(),
                    c.GetRequiredService<ITaskCancellationSourceFactory>()));

            registry.RegisterCallback<IRasDialCallbackHandler>(
                c => new RasDialCallbackHandlerLoggingAdvice(
                    new DefaultRasDialCallbackHandler(
                        c.GetRequiredService<IRasHangUp>(),
                        c.GetRequiredService<IRasEnumConnections>(),
                        c.GetRequiredService<RasDialCallbackExceptionPolicy>(),
                        c.GetRequiredService<IValueWaiter<IntPtr>>(),
                        c.GetRequiredService<ITaskCancellationSourceFactory>()),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            registry.RegisterCallback<IWin32FormatMessage>(
                c => new Win32FormatMessageService(
                    c.GetRequiredService<IKernel32>(),
                    c.GetRequiredService<IMarshaller>()));
        }
    }
}