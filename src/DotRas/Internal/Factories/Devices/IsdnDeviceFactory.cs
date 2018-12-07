using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class IsdnDeviceFactory : IDeviceFactory<Isdn>
    {
        public Isdn Create(string name)
        {
            return new Isdn(name);
        }
    }
}