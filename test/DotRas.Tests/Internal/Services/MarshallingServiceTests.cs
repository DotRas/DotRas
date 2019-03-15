using System;
using System.Runtime.InteropServices;
using DotRas.Internal.Services;
using DotRas.Tests.Stubs;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class MarshallingServiceTests
    {
        [Test]
        public void IdentifyTheCorrectSizeOfAnInteger()
        {
            var target = new MarshallingService();
            var result = target.SizeOf<int>();

            Assert.AreEqual(4, result);
        }

        [Test]
        public void IdentifyTheCorrectSizeOfAStructure()
        {
            var target = new MarshallingService();
            var result = target.SizeOf<StubStructure>();

            Assert.AreEqual(32, result);
        }

        [Test]
        public void FreeTheMemoryAllocated()
        {
            var result = IntPtr.Zero;
            var target = new StubStructMarshallingService();

            try
            {
                result = Marshal.AllocHGlobal(4);
                Assert.AreNotEqual(IntPtr.Zero, result);
            }
            finally
            {
                target.FreeHGlobalIfNeeded(result);

                Assert.IsTrue(target.ReleasedUnmanagedMemory);
            }
        }

        [Test]
        public void NotFreeTheMemoryAllocatedIfPtrIsZero()
        {
            var target = new StubStructMarshallingService();
            target.FreeHGlobalIfNeeded(IntPtr.Zero);

            Assert.IsFalse(target.ReleasedUnmanagedMemory);
        }

        [Test]
        public void AllocateThePointerSpecified()
        {
            var result = IntPtr.Zero;

            try
            {
                var target = new MarshallingService();
                result = target.AllocHGlobal(4);
                
                Assert.AreNotEqual(IntPtr.Zero, result);
            }
            finally 
            {
                if (result != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(result);
                }
            }
        }

        [Test]
        public void MarshalTheValueToAPointer()
        {
            var lpStubStructure = IntPtr.Zero;
            var target = new MarshallingService();

            try
            {
                var sizeOfStubStructure = Marshal.SizeOf<StubStructure>();
                var value = new StubStructure
                {
                    Field1 = 1,
                    Field2 = 2,
                    Field3 = 3,
                    Field4 = "4"
                };

                lpStubStructure = Marshal.AllocHGlobal(sizeOfStubStructure);
                target.StructureToPtr(value, lpStubStructure);

                var result = Marshal.PtrToStructure<StubStructure>(lpStubStructure);

                Assert.AreEqual(1, result.Field1);
                Assert.AreEqual(2, result.Field2);
                Assert.AreEqual(3, result.Field3);
                Assert.AreEqual("4", result.Field4);
            }
            finally
            {
                if (lpStubStructure != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpStubStructure);
                }
            }
        }

        [Test]
        public void ThrowAnExceptionWhenThePtrIsZero()
        {
            var target = new MarshallingService();
            Assert.Throws<ArgumentNullException>(() => target.StructureToPtr(new StubStructure(), IntPtr.Zero));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheSizeIsZero()
        {
            var target = new MarshallingService();
            Assert.Throws<ArgumentException>(() => target.AllocHGlobal(0));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheSizeIsLessThanZero()
        {
            var target = new MarshallingService();
            Assert.Throws<ArgumentException>(() => target.AllocHGlobal(0));
        }

        [Test]
        public void ReturnsNullWhenThePtrIsZero()
        {
            var target = new MarshallingService();
            var result = target.PtrToUnicodeString(IntPtr.Zero, 1);

            Assert.IsNull(result);
        }
        [Test]
        public void ReturnsEmptyStringWhenTheLengthIsZero()
        {
            var target = new MarshallingService();
            var result = target.PtrToUnicodeString(new IntPtr(1), 0);

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void MarshalsTheStringFromThePtrAsExpected()
        {
            var ptr = IntPtr.Zero;
            var expected = "Hello";

            try
            {
                ptr = Marshal.StringToHGlobalUni(expected);

                var target = new MarshallingService();
                var result = target.PtrToUnicodeString(ptr, expected.Length);

                Assert.AreEqual(expected, result);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}