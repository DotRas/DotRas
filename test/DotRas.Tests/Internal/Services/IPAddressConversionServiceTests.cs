using System;
using System.Net;
using DotRas.Internal.Services;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class IPAddressConversionServiceTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheEndPointTypeIsNotSupported()
        {
            var target = new IPAddressConversionService();
            Assert.Throws<NotSupportedException>(() => target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                type = (RASTUNNELENDPOINTTYPE)(-1)
            }));
        }

        [Test]
        public void ConvertsAnIPv4AddressWithMoreBytesThanNeeded()
        {
            var bytes = new byte[] { 127, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var target = new IPAddressConversionService();
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

            var target = new IPAddressConversionService();
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

            var target = new IPAddressConversionService();
            var result = target.ConvertFromEndpoint(new RASTUNNELENDPOINT
            {
                addr = bytes,
                type = RASTUNNELENDPOINTTYPE.IPv6
            });

            Assert.AreEqual("::1", result.ToString());
        }
    }
}