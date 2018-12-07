using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class Sw56DeviceFactory : IDeviceFactory
    {
        public Device Create(string name)
        {
            return new Sw56(name);
        }
    }
}