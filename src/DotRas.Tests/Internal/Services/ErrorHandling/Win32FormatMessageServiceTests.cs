using System.ComponentModel;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.ErrorHandling;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services.ErrorHandling;

[TestFixture]
public class Win32FormatMessageServiceTests
{
    private delegate int FormatMessageCallback(
        int dwFlags,
        IntPtr lpSource,
        int dwMessageId,
        int dwLanguageId,
        ref IntPtr lpBuffer,
        int nSize,
        IntPtr arguments);

    [Test]
    public void ThrowsAnExceptionWhenApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Win32FormatMessageService(null, new Mock<IMarshaller>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenMarshallerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Win32FormatMessageService(new Mock<IKernel32>().Object, null));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheErrorCodeIsZero()
    {
        var api = new Mock<IKernel32>();
        var marshaller = new Mock<IMarshaller>();

        var target = new Win32FormatMessageService(api.Object, marshaller.Object);
        Assert.Throws<ArgumentException>(() => target.FormatMessage(0));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheErrorCodeIsLessThanZer0()
    {
        var api = new Mock<IKernel32>();
        var marshaller = new Mock<IMarshaller>();

        var target = new Win32FormatMessageService(api.Object, marshaller.Object);
        Assert.Throws<ArgumentException>(() => target.FormatMessage(-1));
    }

    [Test]
    public void FormatsTheMessageAsExpected()
    {
        var buffer = new IntPtr(1);

        var api = new Mock<IKernel32>();
        api.Setup(o => o.FormatMessage(It.IsAny<int>(), IntPtr.Zero, 6, 0, ref It.Ref<IntPtr>.IsAny, 0, IntPtr.Zero)).Returns(new FormatMessageCallback(
            (int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, ref IntPtr lpBuffer, int nSize, IntPtr arguments) =>
            {
                Assert.AreEqual(IntPtr.Zero, lpBuffer);

                lpBuffer = buffer;
                return 1;
            }));

        var marshaller = new Mock<IMarshaller>();
        marshaller.Setup(o => o.PtrToUnicodeString(buffer, 1)).Returns("test");

        var target = new Win32FormatMessageService(api.Object, marshaller.Object);
        var result = target.FormatMessage(6);

        Assert.AreEqual("test", result);
        marshaller.Verify(o => o.FreeHGlobalIfNeeded(buffer), Times.Once);
    }

    [Test]
    public void ThrowsAnExceptionWhenTheResultIsZero()
    {
        var api = new Mock<IKernel32>();
        api.Setup(o => o.FormatMessage(It.IsAny<int>(), IntPtr.Zero, 6, 0, ref It.Ref<IntPtr>.IsAny, 0, IntPtr.Zero)).Returns(0);

        var marshaller = new Mock<IMarshaller>();

        var target = new Win32FormatMessageService(api.Object, marshaller.Object);
        Assert.Throws<Win32Exception>(() => target.FormatMessage(6));

        marshaller.Verify(o => o.FreeHGlobalIfNeeded(IntPtr.Zero), Times.Once);
    }
}