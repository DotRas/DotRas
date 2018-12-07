using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class FrameRelayDeviceFactory : IDeviceFactory
    {
        public Device Create(string name)
        {
            return new FrameRelay(name);
        }
    }
}