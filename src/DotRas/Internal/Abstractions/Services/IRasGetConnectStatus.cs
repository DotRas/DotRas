using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetConnectStatus
    {
        RasConnectionStatus GetConnectionStatus(RasHandle handle);
    }
}