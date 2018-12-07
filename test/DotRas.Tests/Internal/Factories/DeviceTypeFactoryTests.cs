using System;
using NUnit.Framework;
using DotRas.Internal.Factories;

namespace DotRas.Tests.Internal.Factories
{
    [TestFixture]
    public class DeviceTypeFactoryTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheServiceLocatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new DeviceTypeFactory(null);
            });
        }
    }
}