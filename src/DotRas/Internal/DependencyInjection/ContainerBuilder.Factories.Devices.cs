using System.ComponentModel.Design;
using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Factories.Devices;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterDeviceFactories(IServiceContainer container)
        {
            container.AddService(typeof(IDeviceFactory<Atm>),
                (c, _) => new AtmDeviceFactory());

            container.AddService(typeof(IDeviceFactory<FrameRelay>),
                (c, _) => new FrameRelayDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Generic>),
                (c, _) => new GenericDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Irda>),
                (c, _) => new IrdaDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Isdn>),
                (c, _) => new IsdnDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Modem>),
                (c, _) => new ModemDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Pad>),
                (c, _) => new PadDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Parallel>),
                (c, _) => new ParallelDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Pppoe>),
                (c, _) => new PppoeDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Serial>),
                (c, _) => new SerialDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Sonet>),
                (c, _) => new SonetDeviceFactory());

            container.AddService(typeof(IDeviceFactory<Sw56>),
                (c, _) => new Sw56DeviceFactory());

            container.AddService(typeof(IDeviceFactory<Vpn>),
                (c, _) => new VpnDeviceFactory());

            container.AddService(typeof(IDeviceFactory<X25>),
                (c, _) => new X25DeviceFactory());
        }
    }
}