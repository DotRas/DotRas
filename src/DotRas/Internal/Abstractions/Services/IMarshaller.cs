using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IMarshaller
    {
        int SizeOf<T>();

        IntPtr GetAddressOfPinnedArrayElement(Array array, int index);
        byte[] ReadDataFromPtr(IntPtr ptr, int length);

        IntPtr AllocHGlobal(int size);
        bool FreeHGlobalIfNeeded(IntPtr ptr);

        void StructureToPtr<T>(T structure, IntPtr ptr);
        T PtrToStructure<T>(IntPtr ptr);

        string PtrToUnicodeString(IntPtr ptr, int length);
    }
}