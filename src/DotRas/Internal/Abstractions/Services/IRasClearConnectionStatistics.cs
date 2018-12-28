namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasClearConnectionStatistics
    {
        void ClearConnectionStatistics(IRasConnection connection);
    }
}