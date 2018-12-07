using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class VpnDeviceFactory : IDeviceFactory<Vpn>
    {
        public Vpn Create(string name)
        {
            return new Vpn(name);
        }
    }
}