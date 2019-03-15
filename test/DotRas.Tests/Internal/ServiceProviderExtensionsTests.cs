using System;
using DotRas.Internal;
using DotRas.Internal.Interop;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal
{
    [TestFixture]
    public class ServiceProviderExtensionsTests
    {
        [Test]
        public void RetrievesTheObjectForTheGenericTypeSpecified()
        {
            var result = new Mock<IRasApi32>();

            var target = new StubServiceProvider(result.Object);
            var actual = target.GetService<IRasApi32>();

            Assert.AreEqual(result.Object, actual);
        }

        [Test]
        public void ReturnNullWhenExpected()
        {
            var target = new StubServiceProvider(null);
            var result = target.GetService<IRasApi32>();

            Assert.IsNull(result);
        }

        [Test]
        public void ThrowsAnExceptionWhenServiceTypeIsNull()
        {
            var serviceProvider = new StubServiceProvider(null);
            Assert.Throws<ArgumentNullException>(() => serviceProvider.GetRequiredService(null));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheServiceIsNotFoundWhenRequired()
        {
            var serviceProvider = new StubServiceProvider(null);
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IRasApi32>());
        }
    }
}