using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Infrastructure.Factories.Devices {
    internal class PppoeDeviceFactory : IDeviceFactory {
        public RasDevice Create(string name) => new Pppoe(name);
    }
}
