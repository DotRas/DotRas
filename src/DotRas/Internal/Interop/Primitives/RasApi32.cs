using System;
using System.Runtime.InteropServices;
using System.Text;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Interop.Primitives
{
    internal class RasApi32 : IRasApi32
    {
        public int RasClearConnectionStatistics(IntPtr hRasConn)
        {
            return UnsafeNativeMethods.RasClearConnectionStatistics(hRasConn);
        }

        public int RasConnectionNotification(IntPtr hRasConn, SafeHandle hEvent, RASCN dwFlags)
        {
            return SafeNativeMethods.RasConnectionNotification(hRasConn, hEvent, dwFlags);
        }

        public int RasEnumConnections(RASCONN[] lpRasConn, ref int lpCb, ref int lpConnections)
        {
            return SafeNativeMethods.RasEnumConnections(lpRasConn, ref lpCb, ref lpConnections);
        }

        public int RasDial(ref RASDIALEXTENSIONS lpRasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS lpRasDialParams, NotifierType dwNotifierType, RasDialFunc2 lpvNotifier, out IntPtr lphRasConn)
        {
            return UnsafeNativeMethods.RasDial(ref lpRasDialExtensions, lpszPhoneBook, ref lpRasDialParams, dwNotifierType, lpvNotifier, out lphRasConn);
        }

        public int RasGetConnectStatus(IntPtr hRasConn, ref RASCONNSTATUS lpRasConnStatus)
        {
            return SafeNativeMethods.RasGetConnectStatus(hRasConn, ref lpRasConnStatus);
        }

        public int RasGetCredentials(string lpszPhoneBook, string lpszEntryName, ref RASCREDENTIALS lpCredentials)
        {
            return SafeNativeMethods.RasGetCredentials(lpszPhoneBook, lpszEntryName, ref lpCredentials);
        }

        public int RasGetEapUserIdentity(string pszPhoneBook, string pszEntry, RASEAPF dwFlags, IntPtr hWnd, out IntPtr ppRasEapUserIdentity)
        {
            return SafeNativeMethods.RasGetEapUserIdentity(pszPhoneBook, pszEntry, dwFlags, hWnd, out ppRasEapUserIdentity);
        }

        public int RasGetEntryDialParams(string lpszPhoneBook, ref RASDIALPARAMS lpDialParams, out bool lpfPassword)
        {
            return SafeNativeMethods.RasGetEntryDialParams(lpszPhoneBook, ref lpDialParams, out lpfPassword);
        }

        public int RasGetErrorString(int uErrorValue, StringBuilder lpszErrorString, int cBufSize)
        {
            return SafeNativeMethods.RasGetErrorString(uErrorValue, lpszErrorString, cBufSize);
        }

        public int RasGetConnectionStatistics(IntPtr hRasConn, ref RAS_STATS lpStatistics)
        {
            return SafeNativeMethods.RasGetConnectionStatistics(hRasConn, ref lpStatistics);
        }

        public int RasHangUp(IntPtr hRasConn)
        {
            return UnsafeNativeMethods.RasHangUp(hRasConn);
        }

        public int RasValidateEntryName(string lpszPhoneBook, string lpszEntryName)
        {
            return SafeNativeMethods.RasValidateEntryName(lpszPhoneBook, lpszEntryName);
        }
    }
}