using System;
using System.Runtime.InteropServices;
using static DotRas.Internal.Interop.ExternDll;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Interop.Primitives
{
    internal static class UnsafeNativeMethods
    {
        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasClearConnectionStatistics(
            IntPtr hRasConn);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasDial(
            [In] ref RASDIALEXTENSIONS lpRasDialExtensions,
            string lpszPhoneBook,
            [In] ref RASDIALPARAMS lpRasDialParams,
            NotifierType dwNotifierType,
            Delegate lpvNotifier,
            out IntPtr lphRasConn);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasHangUp(IntPtr hRasConn);
    }
}