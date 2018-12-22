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
        private readonly IWin32ErrorInformation errorInformation;
        private readonly IIPAddressConverter ipAddressConverter;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IDeviceTypeFactory deviceTypeFactory;

        public RasGetConnectStatusService(IRasApi32 api, IStructFactory structFactory, IWin32ErrorInformation errorInformation, IIPAddressConverter ipAddressConverter, IExceptionPolicy exceptionPolicy, IDeviceTypeFactory deviceTypeFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.errorInformation = errorInformation ?? throw new ArgumentNullException(nameof(errorInformation));
            this.ipAddressConverter = ipAddressConverter ?? throw new ArgumentNullException(nameof(ipAddressConverter));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.deviceTypeFactory = deviceTypeFactory ?? throw new ArgumentNullException(nameof(deviceTypeFactory));
        }

        public RasConnectionStatus GetConnectionStatus(RasHandle handle)
        {
            if (handle == null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            var rasConnStatus = GetConnectionStatusByHandle(handle);

            return new RasConnectionStatus(
                rasConnStatus.rasconnstate,
                CreateErrorInformation(rasConnStatus),
                CreateDevice(rasConnStatus),
                rasConnStatus.szPhoneNumber,
                CreateLocalIPAddress(rasConnStatus),
                CreateRemoteIPAddress(rasConnStatus),
                rasConnStatus.rasconnsubstate);
        }

        private RASCONNSTATUS GetConnectionStatusByHandle(RasHandle handle)
        {
            var rasConnStatus = structFactory.Create<RASCONNSTATUS>();

            var ret = api.RasGetConnectStatus(handle, ref rasConnStatus);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }

            return rasConnStatus;
        }

        private Win32ErrorInformation CreateErrorInformation(RASCONNSTATUS rasConnStatus)
        {
            return errorInformation.CreateFromErrorCode(rasConnStatus.dwError);
        }

        private RasDevice CreateDevice(RASCONNSTATUS rasConnStatus)
        {
            return deviceTypeFactory.Create(rasConnStatus.szDeviceName, rasConnStatus.szDeviceType);
        }

        private IPAddress CreateLocalIPAddress(RASCONNSTATUS rasConnStatus)
        {
            return ipAddressConverter.ConvertFromEndpoint(rasConnStatus.localEndpoint);
        }

        private IPAddress CreateRemoteIPAddress(RASCONNSTATUS rasConnStatus)
        {
            return ipAddressConverter.ConvertFromEndpoint(rasConnStatus.remoteEndpoint);
        }
    }
}