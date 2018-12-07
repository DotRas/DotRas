using System.Text;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.Ras;
using static DotRas.Win32.NativeMethods;

namespace DotRas.Win32.Interop
{
    internal class RasApi32 : IRasApi32
    {
        public int RasEnumConnections(RASCONN[] lpRasConn, ref int lpCb, ref int lpConnections)
        {
            return SafeNativeMethods.RasEnumConnections(lpRasConn, ref lpCb, ref lpConnections);
        }

        public int RasDial(ref RASDIALEXTENSIONS lpRasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS lpRasDialParams, NotifierType dwNotifierType, RasDialFunc2 lpvNotifier, out RasHandle lphRasConn)
        {
            return UnsafeNativeMethods.RasDial(ref lpRasDialExtensions, lpszPhoneBook, ref lpRasDialParams, dwNotifierType, lpvNotifier, out lphRasConn);
        }

        public int RasGetConnectStatus(RasHandle hRasConn, ref RASCONNSTATUS lpRasConnStatus)
        {
            return SafeNativeMethods.RasGetConnectStatus(hRasConn, ref lpRasConnStatus);
        }

        public int RasGetErrorString(int uErrorValue, StringBuilder lpszErrorString, int cBufSize)
        {
            return SafeNativeMethods.RasGetErrorString(uErrorValue, lpszErrorString, cBufSize);
        }

        public int RasHangUp(RasHandle hRasConn)
        {
            return UnsafeNativeMethods.RasHangUp(hRasConn);
        }
    }
}