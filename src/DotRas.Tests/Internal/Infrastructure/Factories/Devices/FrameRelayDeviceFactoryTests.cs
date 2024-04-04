using DotRas.Devices;
using DotRas.Internal.Infrastructure.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Infrastructure.Factories.Devices;

[TestFixture]
public class FrameRelayDeviceFactoryTests
{
    [Test]
    public void ReturnADeviceInstance()
    {
        var target = new FrameRelayDeviceFactory();
        var result = target.Create("Test");

        Assert.AreEqual("Test", result.Name);
        Assert.IsAssignableFrom<FrameRelay>(result);
    }
}