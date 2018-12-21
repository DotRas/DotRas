using System.Runtime.InteropServices;
using System.Text;
using static DotRas.Internal.Interop.ExternDll;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Interop.Primitives
{
    internal static class SafeNativeMethods
    {
        [DllImport(AdvApi32Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocateLocallyUniqueId(
            [Out] out Luid pLuid);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasEnumConnections(
            [In, Out] RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);

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
        public static extern int RasGetCredentials(
            string lpszPhonebook,
            string lpszEntryName,
            [In, Out] ref RASCREDENTIALS lpCredentials);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasValidateEntryName(
            string lpszPhonebook,
            string lpszEntryName);
    }
}