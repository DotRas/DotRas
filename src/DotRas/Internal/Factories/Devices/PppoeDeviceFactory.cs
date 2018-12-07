using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class PppoeDeviceFactory : IDeviceFactory
    {
        public Device Create(string name)
        {
            return new Pppoe(name);
        }
    }
}