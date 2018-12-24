namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetConnectStatus
    {
        RasConnectionStatus GetConnectionStatus(IRasConnection connection);
    }
}