using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasGetLinkStatisticsService : IRasGetLinkStatistics
    {
        private readonly IRasApi32 api;
        private readonly IStructFactory structFactory;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasGetLinkStatisticsService(IRasApi32 api, IStructFactory structFactory, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public RasConnectionStatistics GetLinkStatistics(IRasConnection connection, int subEntryId)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var stats = GetLinkStatisticsByHandle(connection.Handle, subEntryId);

            return new RasConnectionStatistics(
                stats.dwBytesXmited,
                stats.dwBytesRcved,
                stats.dwFramesXmited,
                stats.dwFramesRcved,
                stats.dwCrcErr,
                stats.dwTimeoutErr,
                stats.dwAlignmentErr,
                stats.dwHardwareOverrunErr,
                stats.dwFramingErr,
                stats.dwBufferOverrunErr,
                stats.dwCompressionRatioIn,
                stats.dwCompressionRatioOut,
                stats.dwBps,
                TimeSpan.FromMilliseconds(stats.dwConnectDuration));
        }

        private RAS_STATS GetLinkStatisticsByHandle(IntPtr handle, int subEntryId)
        {
            var statistics = structFactory.Create<RAS_STATS>();

            var ret = api.RasGetLinkStatistics(handle, subEntryId, ref statistics);
            if (ret != SUCCESS)
            {
                throw exceptionPolicy.Create(ret);
            }

            return statistics;
        }
    }
}