using System;
using System.Runtime.InteropServices;
using DotRas.Internal.Abstractions.Services;

#pragma warning disable S1168
#pragma warning disable S1854

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
            if (size < 0)
            {
                throw new ArgumentException("The size must be greater than or equal to zero.");
            }
            else if (size == 0)
            {
                return IntPtr.Zero;
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

        public byte[] PtrToByteArray(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            else if (length == 0)
            {
                return new byte[0];
            }

            var result = new byte[length];
            Marshal.Copy(ptr, result, 0, length);

            return result;
        }

        public IntPtr ByteArrayToPtr(byte[] bytes)
        {
            if (bytes == null)
            {
                return IntPtr.Zero;
            }

            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = AllocHGlobal(bytes.Length);

                Marshal.Copy(bytes, 0, ptr, bytes.Length);

                return ptr;
            }
            catch (Exception)
            {
                FreeHGlobalIfNeeded(ptr);
                throw;
            }
        }
    }
}