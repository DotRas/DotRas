using DotRas.Internal.Abstractions.IoC;
using DotRas.Internal.IoC.Factories.Devices;

namespace DotRas.Internal.IoC
{
    internal static partial class ContainerBuilder
    {
        private static void RegisterDeviceFactories(ICompositionRegistry registry)
        {
            registry.RegisterCallback(
                c => new AtmDeviceFactory());

            registry.RegisterCallback(
                c => new FrameRelayDeviceFactory());

            registry.RegisterCallback(
                c => new GenericDeviceFactory());

            registry.RegisterCallback(
                c => new IrdaDeviceFactory());

            registry.RegisterCallback(
                c => new IsdnDeviceFactory());

            registry.RegisterCallback(
                c => new ModemDeviceFactory());

            registry.RegisterCallback(
                c => new PadDeviceFactory());

            registry.RegisterCallback(
                c => new ParallelDeviceFactory());

            registry.RegisterCallback(
                c => new PppoeDeviceFactory());

            registry.RegisterCallback(
                c => new SerialDeviceFactory());

            registry.RegisterCallback(
                c => new SonetDeviceFactory());

            registry.RegisterCallback(
                c => new Sw56DeviceFactory());

            registry.RegisterCallback(
                c => new VpnDeviceFactory());

            registry.RegisterCallback(
                c => new X25DeviceFactory());
        }
    }
}