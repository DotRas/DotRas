using System;
using System.Runtime.InteropServices;
using DotRas.Internal.Abstractions.Providers;

namespace DotRas.Internal.Providers
{
    internal class StructMarshaller : IStructMarshaller
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
    }
}