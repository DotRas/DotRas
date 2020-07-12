using System;
using System.Text;
using DotRas.Internal.Abstractions.Primitives;
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
            ISafeHandleWrapper hEvent, 
            RASCN dwFlags);

        int RasEnumConnections(
            RASCONN[] lpRasConn,
            ref int lpCb,
            ref int lpConnections);

        int RasEnumDevices(
            RASDEVINFO[] lpRasDevInfo,
            ref int lpCb,
            ref int lpcDevices);

        int RasDial(
            ref RASDIALEXTENSIONS lpRasDialExtensions,
            string lpszPhoneBook,
            ref RASDIALPARAMS lpRasDialParams,
            NotifierType dwNotifierType,
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

        int RasGetEapUserData(
            IntPtr hToken,
            string pzPhoneBook,
            string pzEntry,
            IntPtr pbEapData,
            ref int pdwSizeofEapData);

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