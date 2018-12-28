using System;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasClearConnectionStatisticsService : IRasClearConnectionStatistics
    {
        private readonly IRasApi32 api;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasClearConnectionStatisticsService(IRasApi32 api, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public void ClearConnectionStatistics(IRasConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var ret = api.RasClearConnectionStatistics(connection.Handle);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }
        }
    }
}