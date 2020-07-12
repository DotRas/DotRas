using System;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Security;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Security
{
    [TestFixture]
    public class RasGetEapUserDataServiceTests
    {
        private delegate int RasGetEapUserDataCallback(
            IntPtr hToken,
            string phoneBookPath,
            string entryName,
            IntPtr pbEapData,
            ref int sizeofEapData);

        private Mock<IRasApi32> api;
        private Mock<IExceptionPolicy> exceptionPolicy;
        private Mock<IMarshaller> marshaller;

        [SetUp]
        public void Init()
        {
            api = new Mock<IRasApi32>();
            exceptionPolicy = new Mock<IExceptionPolicy>();
            marshaller = new Mock<IMarshaller>();
        }

        [Test]
        public void ThrowsAnExceptionWhenEntryNameIsNull()
        {
            var target = new RasGetEapUserDataService(api.Object, exceptionPolicy.Object, marshaller.Object);
            Assert.Throws<ArgumentNullException>(() => target.GetEapUserData(IntPtr.Zero, null, @"C:\\Test.pbk"));
        }

        [Test]
        public void ThrowsAnExceptionWhenEntryNameIsWhiteSpace()
        {
            var target = new RasGetEapUserDataService(api.Object, exceptionPolicy.Object, marshaller.Object);
            Assert.Throws<ArgumentNullException>(() => target.GetEapUserData(IntPtr.Zero, "     ", @"C:\\Test.pbk"));
        }

        [Test]
        public void ThrowsAnExceptionWhenPhoneBookPathIsNull()
        {
            var target = new RasGetEapUserDataService(api.Object, exceptionPolicy.Object, marshaller.Object);
            Assert.Throws<ArgumentNullException>(() => target.GetEapUserData(IntPtr.Zero, "Test", null));
        }

        [Test]
        public void ThrowsAnExceptionWhenPhoneBookPathIsWhiteSpace()
        {
            var target = new RasGetEapUserDataService(api.Object, exceptionPolicy.Object, marshaller.Object);
            Assert.Throws<ArgumentNullException>(() => target.GetEapUserData(IntPtr.Zero, "Test", "     "));
        }

        [Test]
        public void ResizesTheStructureAsExpected()
        {
            var ptr = new IntPtr(1);
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";
            var expected = new byte[] { 1 };

            marshaller.Setup(o => o.AllocHGlobal(0)).Returns(IntPtr.Zero);
            marshaller.Setup(o => o.AllocHGlobal(1)).Returns(ptr);

            marshaller.Setup(o => o.PtrToByteArray(ptr, 1)).Returns(expected);

            api.Setup(o => o.RasGetEapUserData(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<string>(), IntPtr.Zero, ref It.Ref<int>.IsAny)).Returns(new RasGetEapUserDataCallback(
                (IntPtr token, string path, string name, IntPtr data, ref int eapData) =>
                {
                    eapData = 1;
                    return ERROR_BUFFER_TOO_SMALL;
                }));

            api.Setup(o => o.RasGetEapUserData(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<string>(), ptr, ref It.Ref<int>.IsAny)).Returns(new RasGetEapUserDataCallback(
                (IntPtr token, string path, string name, IntPtr data, ref int eapData) =>
                {
                    eapData = 1;
                    return SUCCESS;
                }));

            var target = new RasGetEapUserDataService(api.Object, exceptionPolicy.Object, marshaller.Object);
            var result = target.GetEapUserData(IntPtr.Zero, entryName, phoneBookPath);

            marshaller.Verify(o => o.FreeHGlobalIfNeeded(IntPtr.Zero), Times.Once);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void MustFreeTheMemoryAndThrowAnExceptionAsExpected()
        {
            var ptr = new IntPtr(1);

            exceptionPolicy.Setup(o => o.Create(-1)).Returns(new TestException());

            marshaller.Setup(o => o.AllocHGlobal(0)).Returns(ptr);
            api.Setup(o => o.RasGetEapUserData(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IntPtr>(), ref It.Ref<int>.IsAny)).Returns(-1);

            var target = new RasGetEapUserDataService(api.Object, exceptionPolicy.Object, marshaller.Object);
            Assert.Throws<TestException>(() => target.GetEapUserData(IntPtr.Zero, "Test", @"C:\Test.pbk"));

            marshaller.Verify(o => o.FreeHGlobalIfNeeded(ptr), Times.Once);
        }
    }
}