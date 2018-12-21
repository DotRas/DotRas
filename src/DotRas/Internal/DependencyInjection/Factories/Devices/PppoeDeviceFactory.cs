using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.DependencyInjection.Factories.Devices
{
    internal class PppoeDeviceFactory : IDeviceFactory
    {
        public RasDevice Create(string name)
        {
            return new Pppoe(name);
        }
    }
}