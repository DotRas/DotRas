using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.DependencyInjection.Factories.Devices
{
    internal class Sw56DeviceFactory : IDeviceFactory
    {
        public RasDevice Create(string name)
        {
            return new Sw56(name);
        }
    }
}