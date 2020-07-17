using System;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetEapUserData
    {
        bool TryUnsafeGetEapUserData(IntPtr impersonationToken, string entryName, string phoneBookPath, out RASEAPINFO eapInfo);
    }
}