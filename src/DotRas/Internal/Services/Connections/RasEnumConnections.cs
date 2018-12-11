using System;
using System.Collections.Generic;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasEnumConnections : IRasEnumConnections
    {        
        private readonly IRasApi32 api;
        private readonly IDeviceTypeFactory deviceTypeFactory;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IStructArrayFactory structFactory;

        public RasEnumConnections(IRasApi32 api, IDeviceTypeFactory deviceTypeFactory, IExceptionPolicy exceptionPolicy, IStructArrayFactory structFactory)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.deviceTypeFactory = deviceTypeFactory ?? throw new ArgumentNullException(nameof(deviceTypeFactory));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
        }

        public IEnumerable<RasConnection> EnumerateConnections()
        {
            var connections = GetConnections(out var count);

            for (var index = 0; index < count; index++)
            {
                yield return CreateConnection(
                    connections[index]);
            }
        }

        protected virtual RASCONN[] GetConnections(out int count)
        {
            RASCONN[] lpRasConns;
            var retry = false;

            count = 1;

            do
            {
                lpRasConns = structFactory.CreateArray<RASCONN>(count, out var lpCb);

                var ret = api.RasEnumConnections(lpRasConns, ref lpCb, ref count);
                if (ret == ERROR_BUFFER_TOO_SMALL)
                {
                    retry = true;
                }
                else if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }
            } while (retry);

            return lpRasConns;
        }

        protected virtual RasConnection CreateConnection(RASCONN hRasConn)
        {
            var handle = RasHandle.FromPtr(hRasConn.handle);

            var device = deviceTypeFactory.Create(hRasConn.deviceName, hRasConn.deviceType);
            if (device == null)
            {
                throw new InvalidOperationException("The device was not created.");
            }

            return new RasConnection(
                handle,
                device,
                hRasConn.entryName,
                hRasConn.phoneBook);
        }
    }
}