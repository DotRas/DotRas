using System.Threading.Tasks;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDial
    {
        bool IsBusy { get; }
        Task<RasConnection> DialAsync(RasDialContext context);
    }
}