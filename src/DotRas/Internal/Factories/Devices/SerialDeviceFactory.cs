using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class SerialDeviceFactory : IDeviceFactory<Serial>
    {
        public Serial Create(string name)
        {
            return new Serial(name);
        }
    }
}