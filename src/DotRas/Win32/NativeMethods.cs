using System;
using System.Runtime.InteropServices;
using static DotRas.Win32.Lmcons;
using static DotRas.Win32.Ras;
using static DotRas.Win32.StdLib;

namespace DotRas.Win32
{
    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        public struct RASCONN
        {
            [SizeOf]
            public int size;
            public IntPtr handle;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxEntryName + 1)]
            public string entryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType + 1)]
            public string deviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName + 1)]
            public string deviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string phoneBook;
            public int subEntryId;
            public Guid entryId;
            //public NativeMethods.RASCF connectionOptions;
            //public Luid sessionId;
            //public Guid correlationId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        public struct RASCONNSTATUS
        {
            [SizeOf]
            public int dwSize;
            public RasConnectionState rasconnstate;
            public int dwError;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType + 1)]
            public string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName + 1)]
            public string szDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxPhoneNumber + 1)]
            public string szPhoneNumber;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct RASDIALEXTENSIONS
        {
            [SizeOf]
            public int dwSize;
            public RDEOPT dwfOptions;
            public IntPtr hwndParent;
            public IntPtr reserved;
            public IntPtr reserved1;
            public RASEAPINFO RasEapInfo;
            public bool fSkipPppAuth;
            public RASDEVSPECIFICINFO RasDevSpecificInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct RASEAPINFO
        {
            [SizeOf]
            public int dwSizeofEapInfo;
            public IntPtr pbEapInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct RASDEVSPECIFICINFO
        {
            [SizeOf]
            public int dwSize;
            public IntPtr pbDevSpecificInfo;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        public struct RASDIALPARAMS
        {
            [SizeOf]
            public int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxEntryName + 1)]
            public string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxPhoneNumber + 1)]
            public string szPhoneNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxCallbackNumber + 1)]
            public string szCallbackNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = UNLEN + 1)]
            public string szUserName;
            [MaskedValue] [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PWLEN + 1)]
            public string szPassword;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DNLEN + 1)]
            public string szDomain;
            public int dwSubEntry;
            public IntPtr dwCallbackId;
            public int dwIfIndex;
        }

        public delegate bool RasDialFunc2(
            IntPtr dwCallbackId,
            int dwSubEntry,
            IntPtr hrasconn,
            uint message,
            RasConnectionState rascs,
            int dwError,
            int dwExtendedError);
    }
}