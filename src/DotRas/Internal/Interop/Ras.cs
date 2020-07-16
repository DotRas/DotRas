using System;

namespace DotRas.Internal.Interop
{
    internal static class Ras
    {
        public const int RASCS_PAUSED = 0x1000;
        public const int RASCS_DONE = 0x2000;
        public const int RASCSS_DONE = 0x2000;

        public const int RAS_MaxDeviceType = 16;
        public const int RAS_MaxPhoneNumber = 128;
        public const int RAS_MaxIpAddress = 15;
        public const int RAS_MaxIpxAddress = 21;

        public const int RAS_MaxEntryName = 256;
        public const int RAS_MaxDeviceName = 128;
        public const int RAS_MaxCallbackNumber = RAS_MaxPhoneNumber;
        public const int RAS_MaxAreaCode = 10;
        public const int RAS_MaxPadType = 32;
        public const int RAS_MaxX25Address = 200;
        public const int RAS_MaxFacilities = 200;
        public const int RAS_MaxUserData = 200;
        public const int RAS_MaxReplyMessage = 1024;
        public const int RAS_MaxDnsSuffix = 256;

        public const string RASDT_Modem = "modem";
        public const string RASDT_Isdn = "isdn";
        public const string RASDT_X25 = "x25";
        public const string RASDT_Vpn = "vpn";
        public const string RASDT_Pad = "pad";
        public const string RASDT_Generic = "GENERIC";
        public const string RASDT_Serial = "SERIAL";
        public const string RASDT_FrameRelay = "FRAMERELAY";
        public const string RASDT_Atm = "ATM";
        public const string RASDT_Sonet = "SONET";
        public const string RASDT_SW56 = "SW56";
        public const string RASDT_Irda = "IRDA";
        public const string RASDT_Parallel = "PARALLEL";
        public const string RASDT_PPPoE = "PPPoE";

        public enum RASTUNNELENDPOINTTYPE
        {
            Unknown,
            IPv4,
            IPv6
        }

        [Flags]
        public enum RASCF
        {
            AllUsers = 0x1,
            GlobalCreds = 0x2,
            OwnerKnown = 0x4,
            OwnerMatch = 0x8
        }

        [Flags]
        public enum RASCN
        {
            Connection = 0x1,
            Disconnection = 0x2,
            //BandwidthAdded = 0x4,
            //BandwidthRemoved = 0x8,
            //Dormant = 0x10,
            //Reconnection = 0x20,
            //EPDGPacketArrival = 0x40
        }

        [Flags]
        public enum RDEOPT
        {
            None = 0x0,
            //UsePrefixSuffix = 0x1,
            PausedStates = 0x2,
            //IgnoreModemSpeaker = 0x4,
            //SetModemSpeaker = 0x8,
            //IgnoreSoftwareCompression = 0x10,
            //SetSoftwareCompression = 0x20,
            //DisableConnectedUI = 0x40,
            //DisableReconnectUI = 0x80,
            //DisableReconnect = 0x100,
            //NoUser = 0x200,
            //PauseOnScript = 0x400,
            //Router = 0x800,
            //CustomDial = 0x1000,
            //UseCustomScripting = 0x2000
        }

        public enum NotifierType
        {
            RasDialFunc2 = 2
        }

        [Flags]
        public enum RASCM
        {
            None = 0x0,
            UserName = 0x1,
            Password = 0x2,
            Domain = 0x4
        }        
    }
}