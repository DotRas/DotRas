using DotRas.Internal.Services;

namespace DotRas.Tests.Stubs;

internal class StubStructMarshallingService : MarshallingService
{
    public bool ReleasedUnmanagedMemory { get; private set; }

    protected override void FreeHGlobalImpl(IntPtr ptr)
    {
        base.FreeHGlobalImpl(ptr);

        ReleasedUnmanagedMemory = true;
    }
}