using System;
using DotRas.Internal.Services.Dialing;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services.Dialing
{
    [TestFixture]
    public class RasDialExtensionsBuilderTests
    {
        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasDialExtensionsBuilder(null));
        }
    }
}