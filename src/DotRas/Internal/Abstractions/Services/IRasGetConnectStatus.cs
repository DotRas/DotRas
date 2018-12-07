using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetConnectStatus
    {
        ConnectionStatus GetConnectionStatus(RasHandle handle);
    }
}