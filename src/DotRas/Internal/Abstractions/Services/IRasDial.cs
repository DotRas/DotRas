using System;
using System.Threading.Tasks;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDial : IDisposable
    {
        bool IsBusy { get; }
        Task<RasConnection> DialAsync(RasDialContext context);
    }
}