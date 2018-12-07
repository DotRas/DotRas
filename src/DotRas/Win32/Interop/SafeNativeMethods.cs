using System;
using System.Runtime.InteropServices;
using System.Text;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.ExternDll;
using static DotRas.Win32.NativeMethods;

namespace DotRas.Win32.Interop
{
    internal static class SafeNativeMethods
    {
        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetErrorString(
            int uErrorValue,
            StringBuilder lpszErrorString,
            int cBufSize);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetConnectStatus(
            RasHandle hrasconn,
            [In, Out] ref RASCONNSTATUS lpRasConnStatus);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasEnumConnections(
            [In, Out] RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);
    }
}