using DotRas.Internal.Infrastructure.Factories.Devices;

namespace DotRas.Internal.Infrastructure.IoC
{
    static partial class ContainerBuilder
    {
        private static void RegisterDeviceFactories(Container container)
        {
            container.RegisterType(typeof(AtmDeviceFactory));
            container.RegisterType(typeof(FrameRelayDeviceFactory));
            container.RegisterType(typeof(GenericDeviceFactory));
            container.RegisterType(typeof(IrdaDeviceFactory));
            container.RegisterType(typeof(IsdnDeviceFactory));
            container.RegisterType(typeof(ModemDeviceFactory));
            container.RegisterType(typeof(PadDeviceFactory));
            container.RegisterType(typeof(ParallelDeviceFactory));
            container.RegisterType(typeof(PppoeDeviceFactory));
            container.RegisterType(typeof(SerialDeviceFactory));
            container.RegisterType(typeof(SonetDeviceFactory));
            container.RegisterType(typeof(Sw56DeviceFactory));
            container.RegisterType(typeof(VpnDeviceFactory));
            container.RegisterType(typeof(X25DeviceFactory));
        }
    }
}