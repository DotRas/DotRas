using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Dialing;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

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

        [Test]
        public void ConfiguresNoOptionsByDefault()
        {
            var factory = new Mock<IStructFactory>();

            var target = new RasDialExtensionsBuilder(factory.Object);
            var result = target.Build(new RasDialContext());

            Assert.AreEqual(RDEOPT.None, result.dwfOptions);
        }

        [Test]
        public void ConfiguresTheOptionsAsExpected()
        {
            var factory = new Mock<IStructFactory>();

            var target = new RasDialExtensionsBuilder(factory.Object);
            var result = target.Build(new RasDialContext
            {
                Options = new RasDialerOptions
                {
                    UsePrefixSuffix = true
                }
            });

            Assert.True(result.dwfOptions.HasFlag(RDEOPT.UsePrefixSuffix));
        }
    }
}