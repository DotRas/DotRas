using DotRas.Internal.Abstractions.Factories;

namespace DotRas.Internal.Infrastructure.Factories.Devices;

internal class ParallelDeviceFactory : IDeviceFactory
{
    public RasDevice Create(string name)
    {
        return new DotRas.Devices.Parallel(name);
    }
}