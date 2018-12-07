using System.Threading.Tasks;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDial
    {
        bool IsBusy { get; }
        Task<Connection> DialAsync(RasDialContext context);
    }
}