namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasClearLinkStatistics
    {
        void ClearLinkStatistics(IRasConnection connection, int subEntryId);
    }
}