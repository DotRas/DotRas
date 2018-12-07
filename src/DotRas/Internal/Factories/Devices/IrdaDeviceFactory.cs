using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class IrdaDeviceFactory : IDeviceFactory
    {
        public Device Create(string name)
        {
            return new Irda(name);
        }
    }
}