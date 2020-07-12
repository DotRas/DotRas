using System;
using System.Runtime.InteropServices;
using System.Text;
using static DotRas.Internal.Interop.ExternDll;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Interop.Primitives
{
    internal static class SafeNativeMethods
    {
        [DllImport(AdvApi32Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocateLocallyUniqueId(
            [Out] out Luid pLuid);

        [DllImport(Kernel32Dll, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int FormatMessage(
            int dwFlags,
            IntPtr lpSource,
            int dwMessageId,
            int dwLanguageId,
            [In, Out] ref IntPtr lpBuffer,
            int nSize,
            IntPtr arguments);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasConnectionNotification(
            IntPtr hRasConn,
            SafeHandle hEvent,
            RASCN dwFlags);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasEnumConnections(
            [In, Out] RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasEnumDevices(
            [In, Out] RASDEVINFO[] lpRasDevInfo,
            ref int lpCb,
            ref int lpcDevices);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetEapUserData(
            IntPtr hToken,
            string lpszPhoneBook,
            string lpszEntry,
            IntPtr pbEapData,
            ref int pdwSizeOfEapData);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetEntryDialParams(
            string lpszPhoneBook,
            [In, Out] ref RASDIALPARAMS lpDialParams,
            [Out] out bool lpfPassword);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetErrorString(
            int uErrorValue,
            StringBuilder lpszErrorString,
            int cBufSize);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetConnectionStatistics(
            IntPtr hRasConn,
            [In, Out] ref RAS_STATS lpStatistics);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetConnectStatus(
            IntPtr hRasConn,
            [In, Out] ref RASCONNSTATUS lpRasConnStatus);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasGetCredentials(
            string lpszPhoneBook,
            string lpszEntryName,
            [In, Out] ref RASCREDENTIALS lpCredentials);

        [DllImport(RasApi32Dll, CharSet = CharSet.Unicode)]
        public static extern int RasValidateEntryName(
            string lpszPhoneBook,
            string lpszEntryName);
    }
}