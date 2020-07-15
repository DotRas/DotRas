using System;
using System.Net;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasGetConnectStatusService : IRasGetConnectStatus
    {
        private readonly IRasApi32 api;
        private readonly IStructFactory structFactory;
        private readonly IIPAddressConverter ipAddressConverter;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IDeviceTypeFactory deviceTypeFactory;

        public RasGetConnectStatusService(IRasApi32 api, IStructFactory structFactory, IIPAddressConverter ipAddressConverter, IExceptionPolicy exceptionPolicy, IDeviceTypeFactory deviceTypeFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.ipAddressConverter = ipAddressConverter ?? throw new ArgumentNullException(nameof(ipAddressConverter));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.deviceTypeFactory = deviceTypeFactory ?? throw new ArgumentNullException(nameof(deviceTypeFactory));
        }

        public RasConnectionStatus GetConnectionStatus(IRasConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var rasConnStatus = GetConnectionStatusByHandle(connection.Handle);

            return new RasConnectionStatus(
                rasConnStatus.rasconnstate,
                GetErrorCode(rasConnStatus.dwError),
                CreateDevice(rasConnStatus),
                rasConnStatus.szPhoneNumber,
                CreateLocalIpAddress(rasConnStatus),
                CreateRemoteIpAddress(rasConnStatus),
                rasConnStatus.rasconnsubstate);
        }

        private int? GetErrorCode(int dwError)
        {
            if (dwError == SUCCESS)
            {
                return null;
            }

            return dwError;
        }

        private RASCONNSTATUS GetConnectionStatusByHandle(IntPtr handle)
        {
            var rasConnStatus = structFactory.Create<RASCONNSTATUS>();

            var ret = api.RasGetConnectStatus(handle, ref rasConnStatus);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }

            return rasConnStatus;
        }

        private RasDevice CreateDevice(RASCONNSTATUS rasConnStatus)
        {
            return deviceTypeFactory.Create(rasConnStatus.szDeviceName, rasConnStatus.szDeviceType);
        }

        private IPAddress CreateLocalIpAddress(RASCONNSTATUS rasConnStatus)
        {
            return ipAddressConverter.ConvertFromEndpoint(rasConnStatus.localEndpoint);
        }

        private IPAddress CreateRemoteIpAddress(RASCONNSTATUS rasConnStatus)
        {
            return ipAddressConverter.ConvertFromEndpoint(rasConnStatus.remoteEndpoint);
        }
    }
}