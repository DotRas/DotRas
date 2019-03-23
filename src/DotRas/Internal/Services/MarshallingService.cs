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

        public IntPtr GetAddressOfPinnedArrayElement(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            else if (index < 0)
            {
                throw new ArgumentException($"{nameof(index)} must be greater than or equal to zero.", nameof(index));
            }

            return Marshal.UnsafeAddrOfPinnedArrayElement(array, index);
        }

        public byte[] ReadDataFromPtr(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            else if (length < 0)
            {
                throw new ArgumentException($"{nameof(length)} must be greater than or equal to zero.", nameof(length));
            }

            var data = new byte[length];
            Marshal.Copy(ptr, data, 0, length);

            return data;
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

        public T PtrToStructure<T>(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptr));
            }

            return Marshal.PtrToStructure<T>(ptr);
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