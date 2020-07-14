using System;
using System.Runtime.InteropServices;
using static DotRas.Internal.Interop.Lmcons;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.StdLib;

#pragma warning disable S101 // Types should be named in PascalCase

namespace DotRas.Internal.Interop
{
    internal static class NativeMethods
    {
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        public struct RASCONN
        {
            [SizeOf]
            public int dwSize;
            public IntPtr hrasconn;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxEntryName + 1)]
            public string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType + 1)]
            public string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName + 1)]
            public string szDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPhonebook;
            public int dwSubEntry;
            public Guid guidEntry;
            public RASCF dwFlags;
            public Luid luid;
            public Guid guidCorrelationId;
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
            public RASTUNNELENDPOINT localEndpoint;
            public RASTUNNELENDPOINT remoteEndpoint;
            public RasConnectionSubState rasconnsubstate;
        }
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        public struct RASCREDENTIALS
        {
            [SizeOf]
            public int dwSize;
            public RASCM dwMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = UNLEN + 1)]
            public string szUserName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PWLEN + 1)]
            public string szPassword;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DNLEN + 1)]
            public string szDomain;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        public struct RASDEVINFO
        {
            [SizeOf]
            public int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType + 1)]
            public string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName + 1)]
            public string szDeviceName;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct RAS_STATS
        {
            [SizeOf]
            public int dwSize;
            public uint dwBytesXmited;
            public uint dwBytesRcved;
            public uint dwFramesXmited;
            public uint dwFramesRcved;
            public uint dwCrcErr;
            public uint dwTimeoutErr;
            public uint dwAlignmentErr;
            public uint dwHardwareOverrunErr;
            public uint dwFramingErr;
            public uint dwBufferOverrunErr;
            public uint dwCompressionRatioIn;
            public uint dwCompressionRatioOut;
            public uint dwBps;
            public uint dwConnectDuration;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RASTUNNELENDPOINT
        {
            public RASTUNNELENDPOINTTYPE type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] addr;
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