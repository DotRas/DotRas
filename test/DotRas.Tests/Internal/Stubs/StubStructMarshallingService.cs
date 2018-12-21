using System;
using DotRas.Internal.Services;

namespace DotRas.Tests.Internal.Stubs
{
    internal class StubStructMarshallingService : StructMarshallingService
    {
        public bool ReleasedUnmanagedMemory { get; private set; }

        protected override void FreeHGlobalImpl(IntPtr ptr)
        {
            base.FreeHGlobalImpl(ptr);

            ReleasedUnmanagedMemory = true;
        }
    }
}