using System;
using System.Collections.Generic;
using System.Reflection;
using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.DependencyInjection;
using static DotRas.Win32.Ras;

namespace DotRas.Internal.Factories
{
    internal class DeviceTypeFactory : IDeviceTypeFactory
    {
        private static readonly IDictionary<string, Type> LookupTable = new Dictionary<string, Type>()
        {
            { RASDT_Atm, typeof(Atm) },
            { RASDT_FrameRelay, typeof(FrameRelay) },
            { RASDT_Generic, typeof(Generic) },
            { RASDT_Irda, typeof(Irda) },
            { RASDT_Isdn, typeof(Isdn) },
            { RASDT_Modem, typeof(Modem) },
            { RASDT_Pad, typeof(Pad) },
            { RASDT_Parallel, typeof(Parallel) },
            { RASDT_PPPoE, typeof(Pppoe) },
            { RASDT_Serial, typeof(Serial) },
            { RASDT_Sonet, typeof(Sonet) },
            { RASDT_SW56, typeof(Sw56) },
            { RASDT_Vpn, typeof(Vpn) },
            { RASDT_X25, typeof(X25) }
        };

        private readonly IServiceProvider serviceLocator;

        public DeviceTypeFactory(IServiceProvider serviceLocator)
        {
            this.serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
        }

        public Device Create(string name, string deviceType)
        {
            if (string.IsNullOrWhiteSpace(deviceType))
            {
                return null;
            }

            var classType = DetermineClassType(deviceType);
            var deviceFactoryType = DetermineFactoryType(classType);

            return CreateDeviceFromFactory(
                serviceLocator.GetRequiredService(deviceFactoryType), 
                name);
        }

        private Type DetermineClassType(string deviceType)
        {
            foreach (var key in LookupTable.Keys)
            {
                if (string.Equals(key, deviceType, StringComparison.CurrentCultureIgnoreCase))
                {
                    return LookupTable[key];
                }
            }

            return null;
        }

        private Type DetermineFactoryType(Type classType)
        {
            var deviceFactoryType = typeof(IDeviceFactory<>).MakeGenericType(classType);
            if (deviceFactoryType == null)
            {
                throw new InvalidOperationException($"The device factory type for '{classType}' could not be determined.");
            }

            return deviceFactoryType;
        }

        private Device CreateDeviceFromFactory(object factory, string name)
        {
            var method = GetCreateMethod(factory.GetType());

            var result = (Device)method.Invoke(factory, new object[] { name });
            if (result == null)
            {
                throw new InvalidOperationException("The device was not created.");
            }

            return result;
        }

        private MethodInfo GetCreateMethod(Type deviceFactoryType)
        {
            return deviceFactoryType.GetMethod(nameof(IDeviceFactory<Device>.Create));
        }

    }
}