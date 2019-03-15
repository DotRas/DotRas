using System;
using System.Collections.Generic;
using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Infrastructure.Factories.Devices;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Infrastructure.Factories
{
    internal class DeviceTypeFactory : IDeviceTypeFactory
    {
        private static readonly IDictionary<string, Type> LookupTable = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { RASDT_Atm, typeof(AtmDeviceFactory) },
            { RASDT_FrameRelay, typeof(FrameRelay) },
            { RASDT_Generic, typeof(GenericDeviceFactory) },
            { RASDT_Irda, typeof(IrdaDeviceFactory) },
            { RASDT_Isdn, typeof(IsdnDeviceFactory) },
            { RASDT_Modem, typeof(ModemDeviceFactory) },
            { RASDT_Pad, typeof(PadDeviceFactory) },
            { RASDT_Parallel, typeof(ParallelDeviceFactory) },
            { RASDT_PPPoE, typeof(PppoeDeviceFactory) },
            { RASDT_Serial, typeof(SerialDeviceFactory) },
            { RASDT_Sonet, typeof(SonetDeviceFactory) },
            { RASDT_SW56, typeof(Sw56DeviceFactory) },
            { RASDT_Vpn, typeof(VpnDeviceFactory) },
            { RASDT_X25, typeof(X25DeviceFactory) }
        };

        private readonly IServiceProvider serviceLocator;

        public DeviceTypeFactory(IServiceProvider serviceLocator)
        {
            this.serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
        }

        public RasDevice Create(string name, string deviceType)
        {
            if (string.IsNullOrWhiteSpace(deviceType))
            {
                return null;
            }

            var factory = GetFactoryFromDeviceType(deviceType);
            if (factory == null)
            {
                return CreateUnknownDevice(name, deviceType);
            }

            return factory.Create(name);
        }

        private IDeviceFactory GetFactoryFromDeviceType(string deviceType)
        {
            if (LookupTable.TryGetValue(deviceType, out var type))
            {
                return serviceLocator.GetService(type) as IDeviceFactory;
            }

            return null;
        }

        private static RasDevice CreateUnknownDevice(string name, string deviceType)
        {
            return new Unknown(name, deviceType);
        }
    }
}