using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class X25DeviceFactory : IDeviceFactory
    {
        public Device Create(string name)
        {
            return new X25(name);
        }
    }
}