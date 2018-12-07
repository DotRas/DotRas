using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class X25DeviceFactory : IDeviceFactory<X25>
    {
        public X25 Create(string name)
        {
            return new X25(name);
        }
    }
}