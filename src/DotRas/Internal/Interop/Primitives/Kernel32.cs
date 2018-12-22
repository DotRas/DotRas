using System;

namespace DotRas.Internal.Interop.Primitives
{
    internal class Kernel32 : IKernel32
    {
        public int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, ref IntPtr lpBuffer, int nSize, IntPtr arguments)
        {
            return SafeNativeMethods.FormatMessage(dwFlags, lpSource, dwMessageId, dwLanguageId, ref lpBuffer, nSize, arguments);
        }
    }
}