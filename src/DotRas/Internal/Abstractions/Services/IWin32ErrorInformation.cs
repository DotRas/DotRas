namespace DotRas.Internal.Abstractions.Services
{
    internal interface IWin32ErrorInformation
    {
        Win32ErrorInformation CreateFromErrorCode(int errorCode);
    }
}