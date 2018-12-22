using System.Net;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IIPAddressConverter
    {
        IPAddress ConvertFromEndpoint(RASTUNNELENDPOINT endPoint);
    }
}