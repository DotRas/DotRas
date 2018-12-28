using System;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasClearLinkStatisticsService : IRasClearLinkStatistics
    {
        private readonly IRasApi32 api;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasClearLinkStatisticsService(IRasApi32 api, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public void ClearLinkStatistics(IRasConnection connection, int subEntryId)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var ret = api.RasClearLinkStatistics(connection.Handle, subEntryId);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }
        }
    }
}