using System;
using System.Runtime.InteropServices;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.ExternDll;
using static DotRas.Win32.NativeMethods;
using static DotRas.Win32.Ras;

namespace DotRas.Win32.Interop
{
    internal static class UnsafeNativeMethods
    {
        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasDial(
            [In] ref RASDIALEXTENSIONS lpRasDialExtensions,
            string lpszPhoneBook,
            [In] ref RASDIALPARAMS lpRasDialParams,
            NotifierType dwNotifierType,
            Delegate lpvNotifier,
            out RasHandle lphRasConn);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasHangUp(RasHandle hRasConn);
    }
}