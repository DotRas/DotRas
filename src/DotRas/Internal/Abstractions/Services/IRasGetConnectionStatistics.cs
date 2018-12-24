namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetConnectionStatistics
    {
        RasConnectionStatistics GetConnectionStatistics(IRasConnection connection);
    }
}