using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class GenericDeviceFactory : IDeviceFactory
    {
        public Device Create(string name)
        {
            return new Generic(name);
        }
    }
}