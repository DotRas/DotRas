using System;
using DotRas.Internal.Services;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class IPAddressConversionServiceTests
    {
        private IPAddressConversionService target;

        [SetUp]
        public void Setup()
        {
            target = new IPAddressConversionService();
        }

        [Test]
        public void ReturnsNullFromUnknownEndpointType()
        {
            var result = target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                type = RASTUNNELENDPOINTTYPE.Unknown,
                addr = null
            });

            Assert.Null(result);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheEndPointTypeIsNotSupported()
        {
            Assert.Throws<NotSupportedException>(() => target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                type = (RASTUNNELENDPOINTTYPE)(-1)
            }));
        }

        [Test]
        public void ConvertsAnIPv4AddressWithMoreBytesThanNeeded()
        {
            var bytes = new byte[] { 127, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var result = target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                addr = bytes,
                type = RASTUNNELENDPOINTTYPE.IPv4
            });

            Assert.AreEqual("127.0.0.1", result.ToString());
        }

        [Test]
        public void ConvertsAnIPv4AddressWithTheExpectedBytes()
        {
            var bytes = new byte[] { 127, 0, 0, 1 };

            var result = target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                addr = bytes,
                type = RASTUNNELENDPOINTTYPE.IPv4
            });

            Assert.AreEqual("127.0.0.1", result.ToString());
        }

        [Test]
        public void ConvertsAnIPv6AddressWithTheExpectedBytes()
        {
            var bytes = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };

            var result = target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                addr = bytes,
                type = RASTUNNELENDPOINTTYPE.IPv6
            });

            Assert.AreEqual("::1", result.ToString());
        }
    }
}