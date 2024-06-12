using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Infrastructure.Factories.Devices {
    internal class X25DeviceFactory : IDeviceFactory {
        public RasDevice Create(string name) => new X25(name);
    }
}
