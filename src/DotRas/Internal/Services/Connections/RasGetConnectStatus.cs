using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Win32;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.NativeMethods;
using static DotRas.Win32.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasGetConnectStatus : IRasGetConnectStatus
    {
        private readonly IRasApi32 api;
        private readonly IStructFactory structFactory;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IDeviceTypeFactory deviceTypeFactory;

        public RasGetConnectStatus(IRasApi32 api, IStructFactory structFactory, IExceptionPolicy exceptionPolicy, IDeviceTypeFactory deviceTypeFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
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
                deviceTypeFactory.Create(rasConnStatus.szDeviceName, rasConnStatus.szDeviceType),
                rasConnStatus.szPhoneNumber);
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
    }
}