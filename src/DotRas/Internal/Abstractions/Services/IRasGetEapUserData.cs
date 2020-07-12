using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetEapUserData
    {
        byte[] GetEapUserData(IntPtr impersonationToken, string entryName, string phoneBookPath);
    }
}