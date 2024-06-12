using DotRas.Internal.Services;
using DotRas.Tests.Stubs;
using NUnit.Framework;
using System;
using System.Runtime.InteropServices;

namespace DotRas.Tests.Internal.Services {
    [TestFixture]
    public class MarshallingServiceTests {
        [Test]
        public void IdentifyTheCorrectSizeOfAnInteger() {
            var target = new MarshallingService();
            var result = target.SizeOf<int>();

            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void IdentifyTheCorrectSizeOfAStructure() {
            var target = new MarshallingService();
            var result = target.SizeOf<StubStructure>();

            Assert.That(result, Is.EqualTo(32));
        }

        [Test]
        public void FreeTheMemoryAllocated() {
            var result = IntPtr.Zero;
            var target = new StubStructMarshallingService();

            try {
                result = Marshal.AllocHGlobal(4);
                Assert.That(result, Is.Not.EqualTo(IntPtr.Zero));
            }
            finally {
                target.FreeHGlobalIfNeeded(result);

                Assert.That(target.ReleasedUnmanagedMemory, Is.True);
            }
        }

        [Test]
        public void NotFreeTheMemoryAllocatedIfPtrIsZero() {
            var target = new StubStructMarshallingService();
            target.FreeHGlobalIfNeeded(IntPtr.Zero);

            Assert.That(target.ReleasedUnmanagedMemory, Is.False);
        }

        [Test]
        public void AllocateThePointerSpecified() {
            var result = IntPtr.Zero;

            try {
                var target = new MarshallingService();
                result = target.AllocHGlobal(4);

                Assert.That(result, Is.Not.EqualTo(IntPtr.Zero));
            }
            finally {
                if (result != IntPtr.Zero) {
                    Marshal.FreeHGlobal(result);
                }
            }
        }

        [Test]
        public void MarshalTheValueToAPointer() {
            var lpStubStructure = IntPtr.Zero;
            var target = new MarshallingService();

            try {
                var sizeOfStubStructure = Marshal.SizeOf<StubStructure>();
                var value = new StubStructure {
                    Field1 = 1,
                    Field2 = 2,
                    Field3 = 3,
                    Field4 = "4"
                };

                lpStubStructure = Marshal.AllocHGlobal(sizeOfStubStructure);
                target.StructureToPtr(value, lpStubStructure);

                var result = Marshal.PtrToStructure<StubStructure>(lpStubStructure);

                Assert.Multiple(() => {
                    Assert.That(result.Field1, Is.EqualTo(1));
                    Assert.That(result.Field2, Is.EqualTo(2));
                    Assert.That(result.Field3, Is.EqualTo(3));
                    Assert.That(result.Field4, Is.EqualTo("4"));
                });
            }
            finally {
                if (lpStubStructure != IntPtr.Zero) {
                    Marshal.FreeHGlobal(lpStubStructure);
                }
            }
        }

        [Test]
        public void ThrowAnExceptionWhenThePtrIsZero() {
            var target = new MarshallingService();
            Assert.Throws<ArgumentNullException>(() => target.StructureToPtr(new StubStructure(), IntPtr.Zero));
        }

        [Test]
        public void ReturnsNullWhenTheSizeIsZero() {
            var target = new MarshallingService();
            var result = target.AllocHGlobal(0);

            Assert.That(result, Is.EqualTo(IntPtr.Zero));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheSizeIsLessThanZero() {
            var target = new MarshallingService();
            Assert.Throws<ArgumentException>(() => target.AllocHGlobal(-1));
        }

        [Test]
        public void ReturnsNullWhenThePtrIsZero() {
            var target = new MarshallingService();
            var result = target.PtrToUnicodeString(IntPtr.Zero, 1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ReturnsEmptyStringWhenTheLengthIsZero() {
            var target = new MarshallingService();
            var result = target.PtrToUnicodeString(new IntPtr(1), 0);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void MarshalsTheStringFromThePtrAsExpected() {
            var ptr = IntPtr.Zero;
            var expected = "Hello";

            try {
                ptr = Marshal.StringToHGlobalUni(expected);

                var target = new MarshallingService();
                var result = target.PtrToUnicodeString(ptr, expected.Length);

                Assert.That(result, Is.EqualTo(expected));
            }
            finally {
                Marshal.FreeHGlobal(ptr);
            }
        }

        [Test]
        public void ReturnsNullArrayWhenZero() {
            var target = new MarshallingService();
            var result = target.PtrToByteArray(IntPtr.Zero, 0);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ReturnsEmptyArrayWhenLengthIsZero() {
            var target = new MarshallingService();
            var result = target.PtrToByteArray(new IntPtr(1), 0);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void MarshalsThePtrToByteArrayAsExpected() {
            IntPtr ptr = IntPtr.Zero;

            try {
                var bytes = new byte[] { 1, 2, 3, 4 };

                var target = new MarshallingService();
                ptr = target.ByteArrayToPtr(bytes);

                var result = target.PtrToByteArray(ptr, bytes.Length);

                Assert.That(result, Is.EqualTo(bytes).AsCollection);
            }
            finally {
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        [Test]
        public void ReturnsNullPtrWhenNullBytes() {
            var target = new MarshallingService();
            var result = target.ByteArrayToPtr(null);

            Assert.That(result, Is.EqualTo(IntPtr.Zero));
        }

        [Test]
        public void MarshalsTheByteArrayToAPtrAsExpected() {
            IntPtr ptr = IntPtr.Zero;

            try {
                var bytes = new byte[] { 1, 2, 3, 4, 5 };

                var target = new MarshallingService();
                var result = target.ByteArrayToPtr(bytes);

                Assert.That(result, Is.Not.EqualTo(IntPtr.Zero));
            }
            finally {
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }
    }
}
