namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetLinkStatistics
    {
        RasConnectionStatistics GetLinkStatistics(IRasConnection connection, int subEntryId);
    }
}