using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IMarshaller
    {
        int SizeOf<T>();

        IntPtr AllocHGlobal(int size);

        bool FreeHGlobalIfNeeded(IntPtr ptr);

        void StructureToPtr<T>(T structure, IntPtr ptr);

        string PtrToUnicodeString(IntPtr ptr, int length);

        byte[] PtrToByteArray(IntPtr ptr, int length);

        IntPtr ByteArrayToPtr(byte[] bytes);
    }
}