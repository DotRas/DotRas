using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class PppoeDeviceFactory : IDeviceFactory<Pppoe>
    {
        public Pppoe Create(string name)
        {
            return new Pppoe(name);
        }
    }
}