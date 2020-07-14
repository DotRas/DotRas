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
    internal class RasEnumConnectionsService : IRasEnumConnections
    {        
        private readonly IRasApi32 api;
        private readonly IDeviceTypeFactory deviceTypeFactory;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IStructArrayFactory structFactory;
        private readonly IServiceProvider serviceLocator;

        public RasEnumConnectionsService(IRasApi32 api, IDeviceTypeFactory deviceTypeFactory, IExceptionPolicy exceptionPolicy, IStructArrayFactory structFactory, IServiceProvider serviceLocator)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.deviceTypeFactory = deviceTypeFactory ?? throw new ArgumentNullException(nameof(deviceTypeFactory));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
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

        private RASCONN[] GetConnections(out int count)
        {
            RASCONN[] lpRasConn;
            bool retry;

            count = 1;

            do
            {
                retry = false;
                lpRasConn = structFactory.CreateArray<RASCONN>(count, out var lpCb);

                var ret = api.RasEnumConnections(lpRasConn, ref lpCb, ref count);
                if (ret == ERROR_BUFFER_TOO_SMALL)
                {
                    retry = true;
                }
                else if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }
            } while (retry);

            return lpRasConn;
        }

        private RasConnection CreateConnection(RASCONN hRasConn)
        {
            var device = deviceTypeFactory.Create(hRasConn.szDeviceName, hRasConn.szDeviceType);
            if (device == null)
            {
                throw new InvalidOperationException("The device was not created.");
            }

            return new RasConnection(
                hRasConn.hrasconn,
                device,
                hRasConn.szEntryName,
                hRasConn.szPhonebook,
                hRasConn.guidEntry,
                CreateConnectionOptions(hRasConn),
                hRasConn.luid,
                hRasConn.guidCorrelationId,
                serviceLocator);
        }

        private RasConnectionOptions CreateConnectionOptions(RASCONN hRasConn)
        {
            return new RasConnectionOptions(hRasConn.dwFlags);
        }
    }
}