using System;
using System.Runtime.InteropServices;
using System.Security;
using static DotRas.Internal.Interop.ExternDll;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Interop.Primitives
{
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasClearConnectionStatistics(
            IntPtr hRasConn);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasDial(
            [In] IntPtr lpRasDialExtensions,
            string lpszPhoneBook,
            [In] IntPtr lpRasDialParams,
            NotifierType dwNotifierType,
            Delegate lpvNotifier,
            out IntPtr lphRasConn);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasHangUp(IntPtr hRasConn);
    }
}