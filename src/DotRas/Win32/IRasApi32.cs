using DotRas.Win32.SafeHandles;
using System.Text;
using static DotRas.Win32.Ras;
using static DotRas.Win32.NativeMethods;

namespace DotRas.Win32
{
    internal interface IRasApi32
    {
        int RasEnumConnections(
            RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);

        int RasDial(
            ref RASDIALEXTENSIONS lpRasDialExtensions,
            string lpszPhoneBook,
            ref RASDIALPARAMS lpRasDialParams,
            NotifierType dwNotifierType,
            RasDialFunc2 lpvNotifier,
            out RasHandle lphRasConn);

        int RasGetConnectStatus(
            RasHandle hRasConn,
            ref RASCONNSTATUS lpRasConnStatus);

        int RasGetCredentials(
            string lpszPhonebook,
            string lpszEntryName,
            ref RASCREDENTIALS lpCredentials);

        int RasGetErrorString(
            int uErrorValue,
            StringBuilder lpszErrorString,
            int cBufSize);

        int RasHangUp(RasHandle hRasConn);

        int RasValidateEntryName(
            string lpszPhonebook,
            string lpszEntryName);
    }
}