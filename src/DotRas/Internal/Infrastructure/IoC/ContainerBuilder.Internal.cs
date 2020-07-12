using System;
using DotRas.Diagnostics;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Infrastructure.Advice;
using DotRas.Internal.Interop;
using DotRas.Internal.Policies;
using DotRas.Internal.Services;
using DotRas.Internal.Services.Connections;
using DotRas.Internal.Services.Devices;
using DotRas.Internal.Services.Dialing;
using DotRas.Internal.Services.ErrorHandling;
using DotRas.Internal.Services.PhoneBooks;
using DotRas.Internal.Services.Security;

namespace DotRas.Internal.Infrastructure.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterInternal(Container container)
        {
            RegisterPolicies(container);
            RegisterThreading(container);

            container.Register<IAllocateLocallyUniqueId>(typeof(AllocateLocallyUniqueIdService));
            container.Register<IRasEnumConnections>(typeof(RasEnumConnectionsService));
            container.Register<IIPAddressConverter>(typeof(IPAddressConversionService));

            container.Register<IMarshaller>(() =>
                new MarshallerLoggingAdvice(
                    new MarshallingService(),
                    container.GetRequiredService<IEventLoggingPolicy>()));

            container.Register<IPhoneBookEntryValidator>(typeof(PhoneBookEntryNameValidationService));
            container.Register<IRasClearConnectionStatistics>(typeof(RasClearConnectionStatisticsService));
            container.Register<IRasConnectionNotification>(typeof(RasConnectionNotificationService));
            container.Register<IRasConnectionNotificationCallbackHandler>(typeof(RasConnectionNotificationCallbackHandler));
            container.Register<IRasHangUp>(typeof(RasHangUpService));
            container.Register<IRasGetConnectionStatistics>(typeof(RasGetConnectionStatisticsService));
            container.Register<IRasGetEapUserData>(typeof(RasGetEapUserDataService));

            container.Register<IRasGetConnectStatus>(() =>
                new RasGetConnectStatusService(
                    container.GetRequiredService<IRasApi32>(),
                    container.GetRequiredService<IStructFactory>(),
                    container.GetRequiredService<IIPAddressConverter>(),
                    container.GetRequiredService<RasGetConnectStatusExceptionPolicy>(),
                    container.GetRequiredService<IDeviceTypeFactory>()));

            container.Register<IRasGetErrorString>(typeof(RasGetErrorStringService));
            container.Register<IRasGetCredentials>(typeof(RasGetCredentialsService));
            container.Register<IRasDialExtensionsBuilder>(typeof(RasDialExtensionsBuilder));
            container.Register<IRasDialParamsBuilder>(typeof(RasDialParamsBuilder));
            container.Register<IRasDial>(typeof(RasDialService));

            container.Register<IRasDialCallbackHandler>(() =>
                new RasDialCallbackHandlerLoggingAdvice(
                    new DefaultRasDialCallbackHandler(
                        container.GetRequiredService<IRasHangUp>(),
                        container.GetRequiredService<IRasEnumConnections>(),
                        container.GetRequiredService<RasDialCallbackExceptionPolicy>(),
                        container.GetRequiredService<IValueWaiter<IntPtr>>()),
                    container.GetRequiredService<IEventLoggingPolicy>()));

            container.Register<IRasEnumDevices>(typeof(RasEnumDevicesService));

            container.Register<IWin32FormatMessage>(typeof(Win32FormatMessageService));
        }
    }
}