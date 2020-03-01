using System;
using System.Collections.Generic;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Devices
{
    internal class RasEnumDevicesService : IRasEnumDevices
    {
        private readonly IRasApi32 api;
        private readonly IStructArrayFactory structFactory;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IDeviceTypeFactory deviceTypeFactory;

        public RasEnumDevicesService(IRasApi32 api, IStructArrayFactory structFactory, IExceptionPolicy exceptionPolicy, IDeviceTypeFactory deviceTypeFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.deviceTypeFactory = deviceTypeFactory ?? throw new ArgumentNullException(nameof(deviceTypeFactory));
        }

        public IEnumerable<RasDevice> EnumerateDevices()
        {
            var devices = GetDevices(out var count);

            for (var index = 0; index < count; index++)
            {
                yield return CreateDevice(
                    devices[index]);
            }
        }

        private RASDEVINFO[] GetDevices(out int count)
        {
            RASDEVINFO[] lpRasDevInfo;
            bool retry;

            count = 1;

            do
            {
                retry = false;
                lpRasDevInfo = structFactory.CreateArray<RASDEVINFO>(count, out var lpCb);

                var ret = api.RasEnumDevices(lpRasDevInfo, ref lpCb, ref count);
                if (ret == ERROR_BUFFER_TOO_SMALL)
                {
                    retry = true;
                }
                else if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }
            } while (retry);

            return lpRasDevInfo;
        }

        private RasDevice CreateDevice(RASDEVINFO rasDevInfo)
        {
            return deviceTypeFactory.Create(rasDevInfo.szDeviceName, rasDevInfo.szDeviceType);
        }
    }
}