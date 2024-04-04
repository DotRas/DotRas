using System.Runtime.InteropServices;

namespace DotRas.Tests.Stubs;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
public struct StubStructure
{
    public int Field1;
    public int Field2;
    public int Field3;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
    public string Field4;
}