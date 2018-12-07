using DotRas.Devices;
using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Factories.Devices
{
    internal class FrameRelayDeviceFactory : IDeviceFactory<FrameRelay>
    {
        public FrameRelay Create(string name)
        {
            return new FrameRelay(name);
        }
    }
}