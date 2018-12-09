using System.Net;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetCredentials
    {
        NetworkCredential GetNetworkCredential(string entryName, string phoneBookPath);
    }
}