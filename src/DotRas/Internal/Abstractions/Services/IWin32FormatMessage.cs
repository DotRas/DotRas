namespace DotRas.Internal.Abstractions.Services
{
    internal interface IWin32FormatMessage
    {
        string FormatMessage(int errorCode);
    }
}