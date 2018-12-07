using System;
using DotRas.Internal.Providers;

namespace DotRas.Tests.Internal.Stubs
{
    internal class StubStructMarshaller : StructMarshaller
    {
        public bool ReleasedUnmanagedMemory { get; private set; } = false;

        protected override void FreeHGlobalImpl(IntPtr ptr)
        {
            base.FreeHGlobalImpl(ptr);

            ReleasedUnmanagedMemory = true;
        }
    }
}