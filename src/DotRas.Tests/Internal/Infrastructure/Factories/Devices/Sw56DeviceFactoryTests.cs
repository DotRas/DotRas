using DotRas.Devices;
using DotRas.Internal.Infrastructure.Factories.Devices;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Infrastructure.Factories.Devices;

[TestFixture]
public class Sw56DeviceFactoryTests
{
    [Test]
    public void ReturnADeviceInstance()
    {
        var target = new Sw56DeviceFactory();
        var result = target.Create("Test");

        Assert.AreEqual("Test", result.Name);
        Assert.IsAssignableFrom<Sw56>(result);
    }
}