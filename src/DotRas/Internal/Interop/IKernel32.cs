using System;

namespace DotRas.Internal.Interop
{
    internal interface IKernel32
    {
        int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, ref IntPtr lpBuffer, int nSize, IntPtr arguments);
    }
}