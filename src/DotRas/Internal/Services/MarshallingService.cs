using System;
using System.Runtime.InteropServices;
using DotRas.Internal.Abstractions.Services;

namespace DotRas.Internal.Services
{
    internal class MarshallingService : IMarshaller
    {
        public int SizeOf<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }

        public IntPtr AllocHGlobal(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentException("The size must be greater than zero.");
            }

            return Marshal.AllocHGlobal(size);
        }

        public bool FreeHGlobalIfNeeded(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return false;
            }

            FreeHGlobalImpl(ptr);
            return true;
        }

        protected virtual void FreeHGlobalImpl(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }

        public void StructureToPtr<T>(T structure, IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptr));
            }

            Marshal.StructureToPtr(structure, ptr, true);
        }

        public string PtrToUnicodeString(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            else if (length == 0)
            {
                return string.Empty;
            }

            return Marshal.PtrToStringUni(ptr, length);
        }
    }
}