using System;
using System.Runtime.InteropServices;
using System.Text;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Interop
{
    internal interface IRasApi32
    {
        int RasClearConnectionStatistics(
            IntPtr hRasConn);

        int RasConnectionNotification(
            IntPtr hRasConn, 
            SafeHandle hEvent, 
            RASCN dwFlags);

        int RasEnumConnections(
            RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);

        int RasDial(
            ref RASDIALEXTENSIONS lpRasDialExtensions,
            string lpszPhoneBook,
            ref RASDIALPARAMS lpRasDialParams,
            Ras.NotifierType dwNotifierType,
            RasDialFunc2 lpvNotifier,
            out IntPtr lphRasConn);

        int RasGetConnectStatus(
            IntPtr hRasConn,
            ref RASCONNSTATUS lpRasConnStatus);

        int RasGetCredentials(
            string lpszPhoneBook,
            string lpszEntryName,
            ref RASCREDENTIALS lpCredentials);

        int RasGetEntryDialParams(
            string lpszPhoneBook,
            ref RASDIALPARAMS lpDialParams,
            out bool lpfPassword);

        int RasGetErrorString(
            int uErrorValue,
            StringBuilder lpszErrorString,
            int cBufSize);

        int RasGetConnectionStatistics(
            IntPtr hRasConn,
            ref RAS_STATS lpStatistics);

        int RasHangUp(IntPtr hRasConn);

        int RasValidateEntryName(
            string lpszPhoneBook,
            string lpszEntryName);
    }
}