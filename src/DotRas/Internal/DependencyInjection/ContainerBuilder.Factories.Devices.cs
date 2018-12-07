using System.ComponentModel.Design;
using DotRas.Internal.Factories.Devices;

namespace DotRas.Internal.DependencyInjection
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterDeviceFactories(IServiceContainer container)
        {
            container.AddService(typeof(AtmDeviceFactory),
                (c, _) => new AtmDeviceFactory());

            container.AddService(typeof(FrameRelayDeviceFactory),
                (c, _) => new FrameRelayDeviceFactory());

            container.AddService(typeof(GenericDeviceFactory),
                (c, _) => new GenericDeviceFactory());

            container.AddService(typeof(IrdaDeviceFactory),
                (c, _) => new IrdaDeviceFactory());

            container.AddService(typeof(IsdnDeviceFactory),
                (c, _) => new IsdnDeviceFactory());

            container.AddService(typeof(ModemDeviceFactory),
                (c, _) => new ModemDeviceFactory());

            container.AddService(typeof(PadDeviceFactory),
                (c, _) => new PadDeviceFactory());

            container.AddService(typeof(ParallelDeviceFactory),
                (c, _) => new ParallelDeviceFactory());

            container.AddService(typeof(PppoeDeviceFactory),
                (c, _) => new PppoeDeviceFactory());

            container.AddService(typeof(SerialDeviceFactory),
                (c, _) => new SerialDeviceFactory());

            container.AddService(typeof(SonetDeviceFactory),
                (c, _) => new SonetDeviceFactory());

            container.AddService(typeof(Sw56DeviceFactory),
                (c, _) => new Sw56DeviceFactory());

            container.AddService(typeof(VpnDeviceFactory),
                (c, _) => new VpnDeviceFactory());

            container.AddService(typeof(X25DeviceFactory),
                (c, _) => new X25DeviceFactory());
        }
    }
}