using System.ComponentModel.Design;
using DotRas.Diagnostics;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Services;
using DotRas.Internal.Services.Connections;
using DotRas.Internal.Services.Dialing;
using DotRas.Internal.Services.ErrorHandling;
using DotRas.Internal.Services.PhoneBooks;

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
                    c.GetRequiredService<IExceptionPolicy>(),
                    c.GetRequiredService<IStructArrayFactory>(),
                    c));

            container.AddService(typeof(IMarshaller),
                (c, _) => new MarshallerLoggingAdvice(
                    new MarshallingService(),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            container.AddService(typeof(IPhoneBookEntryValidator),
                (c, _) => new PhoneBookEntryNameValidationService(
                    c.GetRequiredService<IRasApi32>()));

            container.AddService(typeof(IRasHangUp),
                (c, _) => new RasHangUpService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IExceptionPolicy>()));

            container.AddService(typeof(IRasGetConnectStatus),
                (c, _) => new RasGetConnectStatusService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IWin32ErrorInformation>(),
                    c.GetRequiredService<IExceptionPolicy>(),
                    c.GetRequiredService<IDeviceTypeFactory>()));

            container.AddService(typeof(IRasGetErrorString),
                (c, _) => new RasGetErrorStringService(
                    c.GetRequiredService<IRasApi32>()));

            container.AddService(typeof(IRasGetCredentials),
                (c, _) => new RasGetCredentialsService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IExceptionPolicy>()));

            container.AddService(typeof(IRasDial),
                (c, _) => new RasDialService(
                    c.GetRequiredService<IRasApi32>(),
                    c.GetRequiredService<IStructFactory>(),
                    c.GetRequiredService<IExceptionPolicy>(),
                    c.GetRequiredService<IRasDialCallbackHandler>(),
                    c.GetRequiredService<ITaskCompletionSourceFactory>()));

            container.AddService(typeof(IRasDialCallbackHandler),
                (c, _) => new RasDialCallbackHandlerLoggingAdvice(
                    new DefaultRasDialCallbackHandler(
                        c.GetRequiredService<IRasHangUp>(),
                        c.GetRequiredService<IRasEnumConnections>(),
                        c.GetRequiredService<IExceptionPolicy>(),
                        c.GetRequiredService<IValueWaiter<RasHandle>>(),
                        c.GetRequiredService<ITaskCancellationSourceFactory>()),
                    c.GetRequiredService<IEventLoggingPolicy>()));

            container.AddService(typeof(IWin32FormatMessage),
                (c, _) => new Win32FormatMessageService(
                    c.GetRequiredService<IKernel32>(),
                    c.GetRequiredService<IMarshaller>()));

            container.AddService(typeof(IWin32ErrorInformation),
                (c, _) => new Win32ErrorInformationService(
                    c.GetRequiredService<IRasGetErrorString>(),
                    c.GetRequiredService<IWin32FormatMessage>()));
        }
    }
}