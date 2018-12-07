using System;

namespace DotRas.Internal.Abstractions.Providers
{
    internal interface IStructMarshaller
    {
        int SizeOf<T>();

        IntPtr AllocHGlobal(int size);
        bool FreeHGlobalIfNeeded(IntPtr ptr);

        void StructureToPtr<T>(T structure, IntPtr ptr);
    }
}