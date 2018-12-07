using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class Sw56DeviceFactory : IDeviceFactory<Sw56>
    {
        public Sw56 Create(string name)
        {
            return new Sw56(name);
        }
    }
}