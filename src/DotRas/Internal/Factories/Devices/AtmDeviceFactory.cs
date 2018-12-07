using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class AtmDeviceFactory : IDeviceFactory<Atm>
    {
        public Atm Create(string name)
        {
            return new Atm(name);
        }
    }
}