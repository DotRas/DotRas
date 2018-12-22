using System;
using System.Net;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Services
{
    internal class IPAddressConversionService : IIPAddressConverter
    {
        public IPAddress ConvertFromEndpoint(RASTUNNELENDPOINT endPoint)
        {
            switch (endPoint.type)
            {
                case RASTUNNELENDPOINTTYPE.IPv4:
                    return CreateIPv4Address(endPoint.addr);

                case RASTUNNELENDPOINTTYPE.IPv6:
                    return CreateIPv6Address(endPoint.addr);

                default:
                    throw new NotSupportedException($"The endpoint type '{endPoint.type}' is not supported.");
            }
        }

        private static IPAddress CreateIPv4Address(byte[] bytes)
        {
            var addressBytes = new byte[4];
            Array.Copy(bytes, 0, addressBytes, 0, 4);

            return new IPAddress(addressBytes);
        }

        private static IPAddress CreateIPv6Address(byte[] bytes)
        {
            return new IPAddress(bytes);
        }
    }
}