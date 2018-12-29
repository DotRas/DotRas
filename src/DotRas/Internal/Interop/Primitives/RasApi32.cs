using System;
using System.Text;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Interop.Primitives
{
    internal class RasApi32 : IRasApi32
    {
        public int RasClearConnectionStatistics(IntPtr hRasConn)
        {
            return UnsafeNativeMethods.RasClearConnectionStatistics(hRasConn);
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

        public int RasGetCredentials(string lpszPhonebook, string lpszEntryName, ref RASCREDENTIALS lpCredentials)
        {
            return SafeNativeMethods.RasGetCredentials(lpszPhonebook, lpszEntryName, ref lpCredentials);
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

        public int RasValidateEntryName(string lpszPhonebook, string lpszEntryName)
        {
            return SafeNativeMethods.RasValidateEntryName(lpszPhonebook, lpszEntryName);
        }
    }
}